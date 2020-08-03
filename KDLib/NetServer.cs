using KDLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KDLib
{
	public static class NetServer
	{
        #region PrivateMembers
        private static TcpListener ListenerCenter;

		private static TcpListener CheckerCenter;

		private static TcpListener ValidCenter;

		private static Dictionary<EndPoint, TcpClient> TCPClients;

		private static Dictionary<EndPoint, TcpClient> OnlineCLients;

		private static Queue<KDCommand> OnlineCommands;
        #endregion

        #region PublicProperies
        public static Dictionary <MachineType, Dictionary <EndPoint, int>> MachineState { get; private set; }

		public static Dictionary<int, EndPoint> PlayerAvailable { get; private set; }
		#endregion

		#region INotifyStaticPropertyChanged
		private static event EventHandler<PropertyChangedEventArgs> StaticPropertiesChanged;

		private static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = "")
		{
			StaticPropertiesChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
		}

		private static void NotifyStaticPropertyChanged(params string[] Names)
		{
			if (StaticPropertiesChanged != null)
			{
				foreach (var propertyName in Names)
				{
					StaticPropertiesChanged(null, new PropertyChangedEventArgs(propertyName));
				}
			}
		}
		#endregion

		public static void Initialize()
        {
			TCPClients = new Dictionary<EndPoint, TcpClient>();
			OnlineCommands = new Queue<KDCommand>();
			OnlineCLients = new Dictionary<EndPoint, TcpClient>();
			PlayerAvailable = new Dictionary<int, EndPoint>();
			MachineState = new Dictionary<MachineType, Dictionary<EndPoint, int>>();
		}

		public static void Start()
		{
			ListenerCenter = new TcpListener(IPAddress.Any, Data.PortForTCP);
			CheckerCenter = new TcpListener(IPAddress.Any, Data.PortForChecker);
			ValidCenter = new TcpListener(IPAddress.Any, Data.PortForValidCheck);

			ListenerCenter.Start();
			CheckerCenter.Start();
			ValidCenter.Start();

			List<Task> tasks = new List<Task>();
			tasks.Add(AcceptClient());
			tasks.Add(AcceptVaid());
			tasks.Add(AcceptChecker());
			Task.WaitAll(tasks.ToArray());
		}

		//Don't change
		private static Task AcceptVaid()
		{
			Action ThisAction = () =>
			{
				int t = 0;
				while (true)
				{
					t = (t > 100) ? 0 : t;
					Console.WriteLine("checking..." + t);
					Task.Delay(1000).Wait();
					t++;
					if (ValidCenter.Pending())
					{
						Console.WriteLine("CHECKED... at " + t);
						ValidCenter.AcceptTcpClient();
					}
				}
			};
			return Task.Factory.StartNew(ThisAction);
		}

		private static Task AcceptChecker()
		{
			Action ThisAction = () =>
			{
				while (true)
				{
					if (ListenerCenter.Pending())
					{
						using (var client = CheckerCenter.AcceptTcpClient())
						{
							OnlineCLients[client.Client.RemoteEndPoint] = client;
							ListenFromChecker(client.Client.RemoteEndPoint);
						}
					}
				}
			};
			return Task.WhenAny(ProcessCommandChecker(), Task.Factory.StartNew(ThisAction));
		}

		private async static void ListenFromChecker(EndPoint Pos)
		{
			Action ThisAction = () =>
			{
				StreamReader ReadFromStream = new StreamReader(OnlineCLients[Pos].GetStream());
				string command;
				while (true)
				{
					command = "";
					try
					{
						command = ReadFromStream.ReadToEnd();
					}
					catch (InvalidOperationException)
					{
						continue;
					}
					catch
					{
						OnlineCLients.Remove(Pos);
						return;
					}
					if (command != null)
						OnlineCommands.Enqueue(JsonConvert.DeserializeObject<KDCommand>(command));
				}
			};
			await Task.Factory.StartNew(ThisAction);
		}

		private static Task ProcessCommandChecker()
        {
			Action ThisAction = () =>
			{
				while (true)
                {
					while (OnlineCommands.Count > 0)
                    {
						KDCommand command = OnlineCommands.Dequeue();
						switch (command.PrefixCmd)
                        {
							case CommandType.Forcusing:
								MachineState[command.Machine][command.ID] = 1;
								break;
							case CommandType.LostForcus:
								MachineState[command.Machine][command.ID] = -1;
								break;
							default:
								break;
                        }
                    }
                }
			};
			return Task.Factory.StartNew(ThisAction);
        }

		private async static Task AcceptClient()
		{
			Action ThisAction = () =>
			{
				TcpClient client = new TcpClient();
				while (true)
				{
					if (ListenerCenter.Pending())
					{
						client = ListenerCenter.AcceptTcpClient();
						TCPClients[client.Client.RemoteEndPoint] = client;
						var list = new ObservableCollection<InfoToChoose>();
						list.Add(new InfoToChoose("abc"));
						list.Add(new InfoToChoose("adbc"));
						list.Add(new InfoToChoose("aadabc"));
						SendCommandToOne(client.Client.RemoteEndPoint, new KDCommand(CommandType.ClientList, client.Client.LocalEndPoint as IPEndPoint, JsonConvert.SerializeObject(list)));
						ListenFromClient(client.Client.RemoteEndPoint);
					}
				}
			};
			await Task.Factory.StartNew(ThisAction);
		}

		private async static void ListenFromClient(EndPoint Pos)
		{
			Action ThisAction = () =>
			{
				StreamReader ReadFromStream = new StreamReader(TCPClients[Pos].GetStream());
				string command;
				while (true)
				{
					command = "";
					try
					{
						command = ReadFromStream.ReadToEnd();
					}
					catch
					{
						TCPClients.Remove(Pos);
						return;
					}
					if (command != null) Data.Commands.Enqueue(JsonConvert.DeserializeObject<KDCommand>(command));
				}
			};
			await Task.WhenAny(Task.Factory.StartNew(ThisAction), ProcessCommandClient());
		}

		private static Task ProcessCommandClient()
        {
			Action ThisAction = () =>
			{
				while (true)
				{
					while (Data.Commands.Count > 0)
					{
						KDCommand command = Data.Commands.Peek();
						switch (Data.Commands.Peek().PrefixCmd)
						{
							case CommandType.AskForConnect:
								if (command.Content == Data.KeyMC || command.Content == Data.KeyViewer)
                                {
									SendCommandToOne(command.ID, new KDCommand(CommandType.RefuseConnect, null));
									goto EndCommand;
								}
								int ID = Data.Matches[Data.CurrentMatchIndex].Players.FindFromName(command.Content).ID;
								if (PlayerAvailable[ID] == null)
                                {
									SendCommandToOne(command.ID, new KDCommand(CommandType.RefuseConnect, null));
									TCPClients[command.ID].Close();
                                }
								else
                                {
									PlayerAvailable[ID] = command.ID;
									SendCommandToOne(command.ID, new KDCommand(CommandType.AccpetConnect, null));
								}
								goto EndCommand;
							EndCommand:
								command = Data.Commands.Dequeue();
								continue;
							default:
								continue;
						}
					}
				}
			};
			return Task.Factory.StartNew(ThisAction);
		}

		public static void SendCommandToOne(EndPoint pos, KDCommand Command)
		{
			try
			{
				SendMessageToOne(JsonConvert.SerializeObject(Command), TCPClients[pos]);
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten();
			}
			catch
			{
				throw;
			}
		}

		private static async void SendMessageToOne(string message, TcpClient client)
		{
			try
			{
				if (client.Client == null) return;
				StreamWriter WriteToStream = new StreamWriter(client.GetStream()) { AutoFlush = true };
				await WriteToStream.WriteLineAsync(message);
				Console.WriteLine(client.Client.RemoteEndPoint.ToString() + " " +  message.Length.ToString());
				Console.WriteLine(message);
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten();
			}
			catch
            {
				throw;
            }
		}

		public static void SendCommandToAll(KDCommand Command)
		{
			try
			{
				SendMessageToAll(JsonConvert.SerializeObject(Command));
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten();
			}
			catch
			{
				throw;
			}
		}

		private static async void SendMessageToAll(string command)
		{
			try
			{
				List<Task> tasks = new List<Task>();
				foreach (var client in TCPClients.Values)
				{
					StreamWriter WriteToStream = new StreamWriter(client.GetStream()) { AutoFlush = true };
					tasks.Add(WriteToStream.WriteAsync(command));
				}
				await Task.WhenAll(tasks);
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten();
			}
			catch
			{
				throw;
			}
		}
	}
}