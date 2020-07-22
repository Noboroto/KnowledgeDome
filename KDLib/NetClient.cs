using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using KDLib.KDException;
using System.IO;
using System;

namespace KDLib
{
    public static class NetClient
    {
		private static TcpClient ThisClient = new TcpClient();

		private static TcpClient OnlClient = new TcpClient();

        public static bool IsServerOnline { get; private set; }

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
				Connect(IPAddress.Parse(ip)).Wait();
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

		public static async Task Connect(IPAddress ServerAddress)
		{
			try
			{
				if (await IsValidConnection(ServerAddress))
                {
					IsServerOnline = true;
					ServerIP = ServerAddress;

					ThisClient.Connect(ServerAddress, Data.PortForTCP);
					OnlClient.Connect(ServerAddress, Data.PortForChecker);

					SendCommand(new KDCommand(CommandType.AskForConnect), OnlClient);
					
					ListenFromServer();
					CheckServer();
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

		private static async void CheckServer()
        {
			while (true)
            {
				try
                {
					if (await IsValidConnection(ServerIP))
					{
						if (!IsServerOnline) Connect(ServerIP).Wait();
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

		public static void SendCommand(KDCommand Command, TcpClient tcp)
		{
			try
            {
				SendMessage(JsonConvert.SerializeObject(Command), tcp).Wait();
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

		private static async void ListenFromServer()
		{
			using (StreamReader ReadFromStream = new StreamReader(ThisClient.GetStream()))
			{
				while (true)
				{
					string information = "";
					try
					{
						information = await ReadFromStream.ReadToEndAsync();
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
		}

		private static async Task SendMessage(string command, TcpClient tcp)
		{
			using (StreamWriter WriteToStream = new StreamWriter(tcp.GetStream()) { AutoFlush = true })
			{
				try
				{
					await WriteToStream.WriteAsync(command);
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
