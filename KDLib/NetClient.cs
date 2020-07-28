using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using KDLib.KDException;
using System.IO;
using System;
using System.Collections.Generic;

namespace KDLib
{
    public static class NetClient
    {
		private static TcpClient ThisClient = new TcpClient();

		private static TcpClient OnlClient = new TcpClient();

        public static bool IsServerOnline { get; private set; }

		public static List<string> ClientComboBoxChoose { get; private set; }

        public static IPAddress ServerIP { get; private set; }

        private static async Task<bool> IsValidConnection(IPAddress ip)
		{
			using (TcpClient tcp = new TcpClient())
			{
				var taskconnect = tcp.ConnectAsync(ip, Data.PortForValidCheck);
				var timer = Task.Delay(500);

				var result = await Task.WhenAny(new[] { taskconnect, timer });
				return result == taskconnect;
			}
		}

		public static void Connect(string ip)
        {
			try
            {
				Connect(IPAddress.Parse(ip));
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

		public static void Connect(IPAddress ServerAddress)
		{
			try
			{
				if (IsValidConnection(ServerAddress).Result)
                {
					IsServerOnline = true;
					ServerIP = ServerAddress;

					ThisClient.Connect(ServerAddress, Data.PortForTCP);

					ListenFromServer();
				}
				else
                {
					IsServerOnline = false;
					ServerIP = IPAddress.Loopback;
					throw new IPNotFoundException();
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

		private static async void CheckOnlineServer()
        {
			while (true)
            {
				try
                {
					if (await IsValidConnection(ServerIP))
					{
						if (!IsServerOnline) Connect(ServerIP);
						IsServerOnline = true;
						SendCommand(new KDCommand((Data.OnFocus) ? CommandType.Forcusing : CommandType.LostForcus),  OnlClient);
					}
					else
					{
						IsServerOnline = false;
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

		private static Task ProcessCommand ()
        {
			Action ThisAction = () =>
			{
				while (true)
                {
					if (Data.Commands.Count > 0)
                    {
						switch (Data.Commands.Peek().PrefixCmd)
                        {
							case CommandType.ClientList:
								ClientComboBoxChoose = JsonConvert.DeserializeObject<List<string>>(Data.Commands.Dequeue().Content);
								continue;
							case CommandType.AccpetConnect:
								OnlClient.Connect(ServerIP, Data.PortForChecker);
								Data.Commands.Dequeue();
								continue;
							case CommandType.RefuseConnect:
								ThisClient.Close();
								return;
							default:
								continue;
                        }
                    }
                }
			};
			return Task.Factory.StartNew(ThisAction);
		}

		public static async void SendCommand(KDCommand Command, TcpClient tcp)
		{
			try
            {
				await SendMessage(JsonConvert.SerializeObject(Command), tcp);
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

		private static Task ListenFromServer()
		{
			Action ThisAction = () =>
			{
				using (StreamReader ReadFromStream = new StreamReader(ThisClient.GetStream()))
				{
					while (true)
					{
						string information = "";
						try
						{
							information = ReadFromStream.ReadToEnd();
						}
						catch (ObjectDisposedException)
						{
							break;
						}
						catch (AggregateException ae)
						{
							throw ae.Flatten();
						}
						catch
						{
							throw;
						}
						if (information != "") Data.Commands.Enqueue(JsonConvert.DeserializeObject<KDCommand>(information));
					}
				}
			};
			return Task.Factory.StartNew(ThisAction);
		}

		private static Task SendMessage(string command, TcpClient tcp)
		{
			Action ThisAction = () => {
				using (StreamWriter WriteToStream = new StreamWriter(tcp.GetStream()) { AutoFlush = true })
				{
					try
					{
						WriteToStream.Write(command);
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
			};
			return Task.Factory.StartNew(ThisAction);
		}
	}
}
