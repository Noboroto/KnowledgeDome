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

namespace AILib
{
	public static class NetHost
	{
		private static TcpListener ListenerCenter;

		private static TcpListener CheckerCenter;

		private static TcpListener ValidCenter;

		private static List<TcpClient> TCPClients;

		private static List<TcpClient> OnlineCLients;

		private static Dictionary<int, int> OnlineStatus;

		private static Queue<KDCommand> OnlineCommands;

		public static void Start()
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

			AcceptClient();
			AcceptVaid();
			AcceptChecker();
		}

		private async static void AcceptVaid()
		{
			while (true)
			{
				if (ValidCenter.Pending())
				{
					await ValidCenter.AcceptTcpClientAsync();
				}
			}
		}

		private async static void AcceptChecker()
		{
			ProcessCommandChecker();
			while (true)
			{
				if (ListenerCenter.Pending())
				{
					OnlineCLients.Add(await CheckerCenter.AcceptTcpClientAsync());
					ListenFromChecker(OnlineCLients.Count - 1);
				}
			}
		}

		private static async void ListenFromChecker(int Pos)
		{
			using (StreamReader ReadFromStream = new StreamReader(OnlineCLients[Pos].GetStream()))
			{
				string command;
				while (true)
				{
					command = "";
					try
					{
						command = await ReadFromStream.ReadToEndAsync();
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
		}

		private static async void ProcessCommandChecker()
        {
			Action action = () =>
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
			await new Task(action);
        }

		private async static void AcceptClient()
		{
			while (true)
			{
				if (ListenerCenter.Pending())
				{
					TCPClients.Add(await ListenerCenter.AcceptTcpClientAsync());
					ListenFromClient(TCPClients.Count - 1);
				}
			}
		}

		private static async void ListenFromClient(int Pos)
		{
			using (StreamReader ReadFromStream = new StreamReader(TCPClients[Pos].GetStream()))
			{
				string command;
				while (true)
				{
					command = "";
					try
					{
						command = await ReadFromStream.ReadToEndAsync();
					}
					catch
					{ 
						TCPClients.RemoveAt(Pos);
						return;
					}
					if (command != null) Data.Commands.Enqueue(JsonConvert.DeserializeObject<KDCommand>(command));
				}
			}
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