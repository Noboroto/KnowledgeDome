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

namespace AILib
{
	public static class NetHost
	{
		private static TcpListener ListenerCenter;

		private static List<TcpClient> Clients;

		private static Dictionary <MachineType, int> _ClientOnline;

		private static Thread AcceptClientThread;

		private static Thread[] ClientListenerThreads;

		private static Thread SendMessageThread;

		public static 


		public static void Start()
		{
			ListenerCenter = new TcpListener(IPAddress.Any, 2644);
			Clients = new List<TcpClient>();
			ClientListenerThreads = new Thread[10];
			ListenerCenter.Start();
			AcceptClient();
		}

		private async static void AcceptClient()
		{
			while (true)
			{
				if (ListenerCenter.Pending())
				{
					Clients.Add(await ListenerCenter.AcceptTcpClientAsync());
				}
			}
		}

		private static void StartListenFromClient(int nClient)
		{
			ClientListenerThreads[nClient] = new Thread(ListenFromClient);
			ClientListenerThreads[nClient].IsBackground = true;
			ClientListenerThreads[nClient].Start(nClient);
		}

		private static void ListenFromClient(int Pos)
		{
			StreamReader ReadFromStream = new StreamReader(Clients[Pos].GetStream());
			while (true)
			{
				byte[] array = new byte[1024];
				int num2 = 0;
				try
				{
					num2 = stream.Read(array, 0, 1024);
				}
				catch
				{
					DataProvider dataProvider = new DataProvider("[", "]", AIObjectBase.TagDefinition('w', "disconnect=" + num.ToString()), haskey: false);
					Commands.Add(dataProvider.Children[0]);
					ClientConnected--;
					break;
				}
				if (num2 == 0)
				{
					break;
				}
				foreach (DataProvider child in new DataProvider("[", "]", GetEncodedMessage(Encoding.GetString(array, 0, num2)), haskey: false).Children)
				{
					Commands.Add(child);
				}
			}
			Clients[num] = null;
		}

		public static async void SendCommand(KDCommand Command)
		{
			try
			{
				await SendMessage(JsonConvert.SerializeObject(Command)).Wait();
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
			for (int i = 0; i < Clients.Count; i++)
			{
				if (Clients[i] != null)
				{
					try
					{
						using (StreamWriter WriteToStream = new StreamWriter(Clients[i].GetStream()) { AutoFlush = true })
						{
							await WriteToStream.WriteAsync(command);
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
			}
		}
	}
}
