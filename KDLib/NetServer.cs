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

namespace KDLib
{
	public static class NetServer
	{
		private static TcpListener ListenerCenter;

		private static TcpListener CheckerCenter;

		private static TcpListener ValidCenter;

		private static Dictionary<EndPoint, TcpClient> TCPClients;

		private static Dictionary<EndPoint, TcpClient> OnlineCLients;

		private static Queue<KDCommand> OnlineCommands;

		public static Dictionary <MachineType, Dictionary <EndPoint, int>> MachineState { get; private set; }

		public static Dictionary<int, EndPoint> PlayerAvailable { get; private set; }

		public async static void Start()
		{
			ListenerCenter = new TcpListener(IPAddress.Any, Data.PortForTCP);
			CheckerCenter = new TcpListener(IPAddress.Any, Data.PortForChecker);
			ValidCenter = new TcpListener(IPAddress.Any, Data.PortForValidCheck);
			
			TCPClients = new Dictionary<EndPoint, TcpClient>();
			OnlineCommands = new Queue<KDCommand>();
			OnlineCLients = new Dictionary<EndPoint, TcpClient>();
			PlayerAvailable = new Dictionary<int, EndPoint>();
			MachineState = new Dictionary<MachineType, Dictionary<EndPoint, int>>();

			ListenerCenter.Start();
			CheckerCenter.Start();
			ValidCenter.Start();

			List<Task> tasks = new List<Task>();
			tasks.Add(AcceptClient());
			tasks.Add(AcceptVaid());
			tasks.Add(AcceptChecker());

			await Task.WhenAny(tasks.ToArray());
		}

		//Don't change
		private static Task AcceptVaid()
		{
			Action ThisAction = () =>
			{
				while (true)
				{
					if (ValidCenter.Pending())
					{
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

		private static Task ListenFromChecker(EndPoint Pos)
		{
			Action ThisAction = () =>
			{
				using (StreamReader ReadFromStream = new StreamReader(OnlineCLients[Pos].GetStream()))
				{
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
				}
			};
			return Task.Factory.StartNew(ThisAction);
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

		private static Task AcceptClient()
		{
			Action ThisAction = () =>
			{
				while (true)
				{
					if (ListenerCenter.Pending())
					{
						using (TcpClient client = ListenerCenter.AcceptTcpClient())
						{
							TCPClients[client.Client.RemoteEndPoint] = client;
							ListenFromClient(client.Client.RemoteEndPoint).Start();
						}
					}
				}
			};
			return Task.Factory.StartNew(ThisAction);
		}

		private static Task ListenFromClient(EndPoint Pos)
		{
			Action ThisAction = () =>
			{
				using (StreamReader ReadFromStream = new StreamReader(TCPClients[Pos].GetStream()))
				{
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
				}
			};
			return Task.WhenAny(Task.Factory.StartNew(ThisAction), ProcessCommandClient());
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
									SendCommandToOne(new KDCommand(CommandType.RefuseConnect, null), command.ID);
									goto EndCommand;
								}
								int ID = Data.Matches[Data.CurrentMatchIndex].Players.FindFromName(command.Content).ID;
								if (PlayerAvailable[ID] == null)
                                {
									SendCommandToOne(new KDCommand(CommandType.RefuseConnect, null), command.ID);
									TCPClients[command.ID].Close();
                                }
								else
                                {
									PlayerAvailable[ID] = command.ID;
									SendCommandToOne(new KDCommand(CommandType.AccpetConnect, null), command.ID);
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

		public static void SendCommandToOne(KDCommand Command, EndPoint pos)
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
				using (StreamWriter WriteToStream = new StreamWriter(client.GetStream()) { AutoFlush = true })
				{
					await WriteToStream.WriteAsync(message);
				}
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
					using (StreamWriter WriteToStream = new StreamWriter(client.GetStream()) { AutoFlush = true })
					{
						tasks.Add(WriteToStream.WriteAsync(command));
					}
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