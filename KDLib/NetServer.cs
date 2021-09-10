using GalaSoft.MvvmLight.Messaging;

using KDLib.MessageForUI;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace KDLib
{
	public static class NetServer
	{
		#region PrivateMembers
		private static TcpListener ListenerCenter;

		private static TcpListener ValidCenter;
		#endregion

		#region PublicProperies
		public static CancellationTokenSource tokenSource;
		public static Dictionary<int, ClientInfo> PlayerAvailable { get; private set; }
		public static List<ClientInfo> TCPClients;
		public static List<ClientInfo> MCAvailable { get; private set; }
		#endregion

		public static void Initialize()
		{
			MCAvailable = new List<ClientInfo>();
			tokenSource = new CancellationTokenSource();
			TCPClients = new List<ClientInfo>();
			PlayerAvailable = new Dictionary<int, ClientInfo>();
			ListenerCenter = new TcpListener(IPAddress.Any, Data.PortForTCP);
			ValidCenter = new TcpListener(IPAddress.Any, Data.PortForValidCheck);
			Start();
		}

		public static async void Start()
		{
			ListenerCenter.Start();
			ValidCenter.Start();
			CommandChecker();

			List<Task> tasks = new List<Task> { AcceptClient(), AcceptVaid() };
			try
			{
				await Task.WhenAny(tasks.ToArray());
			}
			catch (AggregateException ae)
			{
				throw ae.Flatten();
			}
			catch (OperationCanceledException)
			{
				return;
			}
			catch
			{
			}
		}

		private static Task AcceptVaid()
		{
			Action ThisAction = () =>
			{
				while (true)
				{
					Task.Delay(1000).Wait();
					if (ValidCenter.Pending())
					{
						ValidCenter.AcceptTcpClient();
					}
				}
			};
			return Task.Run(ThisAction, tokenSource.Token);
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
						var RemoteIP = (client.Client.RemoteEndPoint as IPEndPoint).Address;
						var pos = TCPClients.Count;
						TCPClients.Add(new ClientInfo(client, pos));
						SendCommandToOne(pos, new KDCommand(CommandType.ConfirmIP, RemoteIP, pos, pos.ToString()));
						SendCommandToOne(pos, new KDCommand(CommandType.ClientList, Data.CurrentMatchIndex.ToString()));
						Messenger.Default.Send(new LogMess(KDLogger.Info($"{RemoteIP};{pos} đã kết nối")));
						ListenFromClient(pos);
					}
				}
			};
			await Task.Run(ThisAction);
		}

		private static void ListenFromClient(int pos)
		{
			Task.Run(() =>
			{
				string information;
				StreamReader ReadFromStream = new StreamReader(TCPClients[pos].GetStream());
				while (true)
				{
					information = "";
					try
					{
						information = ReadFromStream.ReadLine();
					}
					catch (IOException)
					{
						Messenger.Default.Send(new LogMess(KDLogger.Info($"{TCPClients[pos].IP};{pos} mất kết nối")));
						TCPClients[pos].Close();
						return;
					}
					if (KDCommand.FromJson(information) != null)
					{
						var command = KDCommand.FromJson(information);
						switch (command.PrefixCmd)
						{
							case CommandType.AskForConnect:
								Data.NetCommands.Enqueue(command);
								break;
							case CommandType.ServerToMC:
							case CommandType.MCToMC:
								Data.FrameCommands.Enqueue(command);
								break;
						}
					}
				}
			});
		}

		private static void CommandChecker()
		{
			Task.Run(() =>
			{
				while (true)
				{
					while (Data.NetCommands.Count > 0)
					{
						KDCommand command = Data.NetCommands.Peek();
						switch (Data.NetCommands.Peek().PrefixCmd)
						{
							case CommandType.AskForConnect:
								var type = Data.GetMachineFromID(int.Parse(command.Content));
								switch (type)
								{
									case Machine.MC:
										SendCommandToOne(command.Pos, new KDCommand(CommandType.AccpetConnect, null));
										Messenger.Default.Send(new LogMess(KDLogger.Info($"Đã kết nối vào MC", LogType.MC, $"{command.OwnIP};{command.Pos}")));
										MCAvailable.Add(TCPClients[command.Pos]);
										goto EndCommand;
									case Machine.Viewer:
										SendCommandToOne(command.Pos, new KDCommand(CommandType.AccpetConnect, null));
										Messenger.Default.Send(new LogMess(KDLogger.Info($"Đã kết nối vào Viewer", LogType.Viewer, $"{command.OwnIP};{command.Pos}")));
										goto EndCommand;
									case Machine.Player:
										int ID = int.Parse(command.Content);
										if (PlayerAvailable.ContainsKey(ID) && PlayerAvailable[ID].Connected)
										{
											SendCommandToOne(command.Pos, new KDCommand(CommandType.RefuseConnect, null));
											Messenger.Default.Send(new LogMess(KDLogger.Info($"Bị từ chối kết nối vào {Data.CurrentMatch.Players[ID].Name}", LogType.Player, $"{command.OwnIP};{command.Pos}")));
										}
										else
										{
											SendCommandToOne(command.Pos, new KDCommand(CommandType.AccpetConnect, null));
											PlayerAvailable[ID] = TCPClients[command.Pos];
											Messenger.Default.Send(new LogMess(KDLogger.Info($"Đã kết nối vào {Data.CurrentMatch.Players[ID].Name}", LogType.Player, $"{command.OwnIP};{command.Pos}")));
										}
										goto EndCommand;
								}
							EndCommand:
								if (Data.NetCommands.Count > 0) Data.NetCommands.Dequeue();
								continue;
							default:
								continue;
						}
					}
				}
			}, tokenSource.Token);
		}

		public static async void SendCommandToOne(TcpClient client, KDCommand Command)
		{
			if (Data.ThisMacineType != Machine.Server) return;
			try
			{
				await SendMessageToOne(Command.ToJson(), client);
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

		public static async void SendCommandToOne(int pos, KDCommand Command)
		{
			if (Data.ThisMacineType != Machine.Server) return;
			try
			{
				await SendMessageToOne(Command.ToJson(), TCPClients[pos].Client);
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

		private static Task SendMessageToOne(string message, TcpClient client)
		{
			return Task.Run(async () =>
			{
				try
				{
					if (!client.Connected) return;
					StreamWriter WriteToStream = new StreamWriter(client.GetStream()) { AutoFlush = true };
					await WriteToStream.WriteLineAsync(message);
				}
				catch (AggregateException ae)
				{
					throw ae.Flatten();
				}
				catch
				{
					throw;
				}
			});
		}

		public static async void SendCommandToAll(KDCommand Command)
		{
			if (Data.ThisMacineType != Machine.Server) return;
			try
			{
				List<Task> tasks = new List<Task>();
				foreach (var client in TCPClients)
				{
					tasks.Add(SendMessageToOne(Command.ToJson(), client.Client));
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

		public static List<string> GetLocalIPAddress()
		{
			var result = new List<string>();
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					result.Add(ip.ToString());
				}
			}
			return result;
		}
	}
}