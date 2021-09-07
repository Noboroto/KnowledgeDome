using GalaSoft.MvvmLight.Messaging;

using KDLib.MessageForUI;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KDLib
{
	public static class NetServer
	{
		#region PrivateMembers
		private static TcpListener ListenerCenter;

		private static TcpListener ValidCenter;

		private static Dictionary<IPAddress, TcpClient> TCPClients;
		#endregion

		#region PublicProperies
		public static CancellationTokenSource tokenSource;
		public static Dictionary<int, IPAddress> PlayerAvailable { get; private set; }
		public static List<IPAddress> MCAvailable { get; private set; }
		#endregion

		public static void Initialize()
		{
			MCAvailable = new List<IPAddress>();
			tokenSource = new CancellationTokenSource();
			TCPClients = new Dictionary<IPAddress, TcpClient>();
			PlayerAvailable = new Dictionary<int, IPAddress>();
			ListenerCenter = new TcpListener(IPAddress.Any, Data.PortForTCP);
			ValidCenter = new TcpListener(IPAddress.Any, Data.PortForValidCheck);
			Start();
		}

		public static async void Start()
		{
			ListenerCenter.Start();
			ValidCenter.Start();
			ProcessCommandClient();

			List<Task> tasks = new List<Task> { AcceptClient(), AcceptVaid() };
			try
			{
				await Task.WhenAny(tasks.ToArray());
			}
			catch (AggregateException)
			{
				MessageBox.Show("alalala");
			}
			catch (OperationCanceledException)
			{
				return;
			}
			catch
			{
				MessageBox.Show("aodalsdjkj");
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
						var remote = (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
						TCPClients[IPAddress.Parse(remote)] = client;
						var Role = new List<string>();
						foreach (var x in Data.CurrentMatch.Players)
						{
							Role.Add(x.Name);
						};
						Role.Add("MC");
						Role.Add("Khán giả");
						Messenger.Default.Send(new LogMess(KDLogger.Info($"{remote} đã kết nối")));
						SendCommandToOne(IPAddress.Parse(remote), new KDCommand(CommandType.ClientList, Data.ToJson(Role)));
						ListenFromClient(IPAddress.Parse(remote));
					}
				}
			};
			await Task.Run(ThisAction);
		}

		private static void ListenFromClient(IPAddress Pos)
		{
			Task.Run(() =>
			{
				string command;
				StreamReader ReadFromStream = new StreamReader(TCPClients[Pos].GetStream());
				while (true)
				{
					command = "";
					try
					{
						command = ReadFromStream.ReadLine();
						Messenger.Default.Send(new LogMess(KDLogger.Info(command)));
					}
					catch (IOException)
					{
						Messenger.Default.Send(new LogMess(KDLogger.Info($"{Pos} mất kết nối")));
						break;
					}
					if (KDCommand.FromJson(command) != null)
						Data.Commands.Enqueue(KDCommand.FromJson(command));
				}
			});
		}

		private static void ProcessCommandClient()
		{
			Task.Run(() =>
			{
				while (true)
				{
					if (tokenSource.Token.IsCancellationRequested) break;
					while (Data.Commands.Count > 0)
					{
						try
						{
							KDCommand command = Data.Commands.Peek();
							switch (Data.Commands.Peek().PrefixCmd)
							{
								case CommandType.AskForConnect:
									var type = Data.GetMachineFromID(int.Parse(command.Content));
									switch (type)
									{
										case Machine.MC:
											Messenger.Default.Send(new LogMess(KDLogger.Info($"Đã kết nối vào MC", LogType.MC, command.OwnIP.ToString())));
											SendCommandToOne(command.OwnIP, new KDCommand(CommandType.AccpetConnect, null));
											MCAvailable.Add(command.OwnIP);
											goto EndCommand;
										case Machine.Viewer:
											Messenger.Default.Send(new LogMess(KDLogger.Info($"Đã kết nối vào Viewer", LogType.Viewer, command.OwnIP.ToString())));
											SendCommandToOne(command.OwnIP, new KDCommand(CommandType.AccpetConnect, null));
											goto EndCommand;
										case Machine.Player:
											int ID = int.Parse(command.Content);
											if (PlayerAvailable.ContainsKey(ID))
											{
												Messenger.Default.Send(new LogMess(KDLogger.Info($"Bị từ chối kết nối vào {Data.CurrentMatch.Players[ID].Name}", LogType.Player, command.OwnIP.ToString())));
												SendCommandToOne(command.OwnIP, new KDCommand(CommandType.RefuseConnect, null));
											}
											else
											{
												PlayerAvailable[ID] = command.OwnIP;
												Messenger.Default.Send(new LogMess(KDLogger.Info($"Đã kết nối vào {Data.CurrentMatch.Players[ID].Name}", LogType.Player, command.OwnIP.ToString())));
												SendCommandToOne(command.OwnIP, new KDCommand(CommandType.AccpetConnect, null));
											}
											goto EndCommand;
									}
								EndCommand:
									if (Data.Commands.Count > 0) Data.Commands.Dequeue();
									continue;
								default:
									continue;
							}
						}
						catch (NullReferenceException)
						{
							continue;
						}
						catch (InvalidOperationException)
						{
							continue;
						}
					}
				}
			}, tokenSource.Token);
		}

		public static async void SendCommandToOne(IPAddress pos, KDCommand Command)
		{
			try
			{
				await SendMessageToOne(Command.ToJson(), TCPClients[pos]);
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
					if (client.Client == null) return;
					if (!client.Client.Connected) return;
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

		public static void SendCommandToAll(KDCommand Command)
		{
			try
			{
				SendMessageToAll(Command.ToJson());
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
				if (TCPClients == null) return;
				foreach (var client in TCPClients.Values)
				{
					StreamWriter WriteToStream = new StreamWriter(client.GetStream()) { AutoFlush = true };
					tasks.Add(WriteToStream.WriteAsync(command));
				}
				await Task.WhenAll(tasks);
			}
			catch (NullReferenceException)
			{
				return;
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