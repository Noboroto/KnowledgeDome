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

		private static Queue<Tuple<KDCommand,int>> OnlineCommands;

		public static void Start()
		{
			ListenerCenter = new TcpListener(IPAddress.Any, Data.PortForTCP);
			CheckerCenter = new TcpListener(IPAddress.Any, Data.PortForChecker);
			ValidCenter = new TcpListener(IPAddress.Any, Data.PortForValidCheck);
			
			TCPClients = new List<TcpClient>();
			OnlineCommands = new Queue<Tuple<KDCommand, int>>();
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
					catch
					{
						OnlineCLients.RemoveAt(Pos);
						return;
					}
					if (command != null)
						OnlineCommands.Enqueue(new Tuple<KDCommand, int>(JsonConvert.DeserializeObject<KDCommand>(command), Pos));
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
						Tuple<KDCommand,int> command = OnlineCommands.Dequeue();
						switch (command.Item1.Machine)
                        {
							case MachineType.Player:
								switch (command.Item1.PrefixCmd)
                                {
									case CommandType.AskForConnect:
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

		public static void SendCommand(KDCommand Command)
		{
			try
			{
				SendMessage(JsonConvert.SerializeObject(Command));
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

		private static async void SendMessage(string command)
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