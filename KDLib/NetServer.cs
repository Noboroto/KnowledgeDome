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

		private static List<TcpClient> TCPClients;

		private static List<TcpClient> OnlineCLients;

		private static Dictionary<int, int> OnlineStatus;

		private static Queue<KDCommand> OnlineCommands;

		public async static void Start()
		{
			ListenerCenter = new TcpListener(IPAddress.Any, Data.PortForTCP);
			CheckerCenter = new TcpListener(IPAddress.Any, Data.PortForChecker);
			ValidCenter = new TcpListener(IPAddress.Any, Data.PortForValidCheck);
			
			TCPClients = new List<TcpClient>();
			OnlineCommands = new Queue<KDCommand>();
			OnlineCLients = new List<TcpClient>();
			
			ListenerCenter.Start();
			CheckerCenter.Start();
			ValidCenter.Start();

			List<Task> tasks = new List<Task>();
			tasks.Add(AcceptClient());
			tasks.Add(AcceptVaid());
			tasks.Add(AcceptChecker());

			await Task.WhenAny(tasks.ToArray());
		}

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
						OnlineCLients.Add(CheckerCenter.AcceptTcpClient());
						ListenFromChecker(OnlineCLients.Count - 1);
					}
				}
			};
			return Task.WhenAny(ProcessCommandChecker(), Task.Factory.StartNew(ThisAction));
		}

		private static Task ListenFromChecker(int Pos)
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
							OnlineCLients[Pos].Close();
							OnlineCLients.RemoveAt(Pos);
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
						switch (command.Machine)
                        {
							case MachineType.Player:
								switch (command.PrefixCmd)
                                {
									case CommandType.Forcusing:
										OnlineStatus[command.ID] = 1;
										break;
									case CommandType.LostForcus:
										OnlineStatus[command.ID] = -1;
										break;
									default:
										break;
                                }
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
						TCPClients.Add(ListenerCenter.AcceptTcpClient());
						ListenFromClient(TCPClients.Count - 1).Start();
					}
				}
			};
			return Task.Factory.StartNew(ThisAction);
		}

		private static Task ListenFromClient(int Pos)
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
							TCPClients.RemoveAt(Pos);
							return;
						}
						if (command != null) Data.Commands.Enqueue(JsonConvert.DeserializeObject<KDCommand>(command));
					}
				}
			};
			return Task.Factory.StartNew(ThisAction);
		}

		public static void SendCommandToOne(KDCommand Command, int pos)
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
				foreach (var client in TCPClients)
				{
					if (client.Client == null) continue;
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