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

		private static IPAddress _ServerIP;

		private static bool _IsServerOnline;

		public static bool IsServerOnline
        {
			get
            {
				return _IsServerOnline;
            }
        }

		public static IPAddress ServerIP
        {
			get
            {
				return _ServerIP;
            }
        }

		private static async Task<bool> IsValidConnection(IPAddress ip, int iPort)
		{
			using (TcpClient tcp = new TcpClient())
			{
				var taskconnect = tcp.ConnectAsync(ip, iPort);
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
				if (await IsValidConnection(ServerAddress, Data.PortForChecker))
                {
					_IsServerOnline = true;
					_ServerIP = ServerAddress;

					ThisClient.Connect(ServerAddress, Data.PortForTCP);
					ThisClient.Connect(ServerAddress, Data.PortForChecker);

					SendCommand(new KDCommand(CommandType.AskForConnect), ThisClient);
					
					ListenFromServer();
				}
				else
                {
					_IsServerOnline = false;
					_ServerIP = IPAddress.Loopback;
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

		private static async void CheckServer ()
        {
			while (true)
            {
				try
                {
					if (await IsValidConnection(ServerIP, Data.PortForChecker))
					{

						_IsServerOnline = true;
						SendCommand(new KDCommand((Data.OnFocus) ? CommandType.Forcusing : CommandType.LostForcus),  OnlClient);
					}
					else
					{
						_IsServerOnline = false;
					}
				}
				catch (AggregateException ae)
				{
					throw ae.Flatten();
				}
				catch
                {
					continue;
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
					catch
					{
						continue;
					}
					if (information != "")
					{
						Data.Commands.Add(JsonConvert.DeserializeObject<KDCommand>(information));
						continue;
					}
					break;
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
