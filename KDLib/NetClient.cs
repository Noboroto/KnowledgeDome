using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KDLib
{
    public static class NetClient
    {
		private static TcpClient tcClient = new TcpClient();

		private static Thread tListenFromServer;

		private static Thread tSendMessage;

		private static async Task<bool> IsConnected(IPAddress ip, int iPort)
		{
			using (TcpClient tcp = new TcpClient())
			{
				var taskconnect = tcp.ConnectAsync(ip, iPort);
				var timer = Task.Delay(500);

				var result = await Task.WhenAny(new[] { taskconnect, timer });
				return result == taskconnect;
			}
		}

		public static async Task Connect(string ip)
        {
			try
            {
				await Connect(IPAddress.Parse(ip));
            }
			catch (Exception e)
            {
				throw new Exception(e.Message);
            }
        }

		public static async Task Connect(IPAddress ServerAddress)
		{
			try
			{
				if (await IsConnected(ServerAddress, Data.PortForChecker))
                {
					tcClient.Connect(ServerAddress, Data.PortForTCP);
					//NetworkStream stream = tcClient.GetStream();
					//byte[] bytes = KDConvert.UTF8Encoder.GetBytes(ClientID.ToString());
					//stream.Write(bytes, 0, bytes.Length);
					//Thread_ListenFromServer();
				}
				else
                {
					throw new Exception("Lỗi rồi!");
                }
			}
			catch (Exception e)
            {
				throw new Exception(e.Message);
            }
		}

		private static void Thread_ListenFromServer()
		{
			tListenFromServer = new Thread(ListenFromServer);
			tListenFromServer.IsBackground = true;
			tListenFromServer.Start();
		}

		public static void SendCommand(KDCommand Command)
		{
			
			tSendMessage = new Thread(SendMessage);
			tSendMessage.IsBackground = true;
			tSendMessage.Start(JsonConvert.SerializeObject(Command));
		}

		private static void ListenFromServer()
		{
			NetworkStream stream = tcClient.GetStream();
			while (true)
			{
				byte[] array = new byte[1024];
				int num = 0;
				try
				{
					num = stream.Read(array, 0, 1024);
				}
				catch (Exception e)
				{
					throw e;
				}
				if (num != 0)
				{
					Data.Commands.Add(JsonConvert.DeserializeObject<KDCommand>(KDConvert.UTF8Encoder.GetString(array)));
					continue;
				}
				break;
			}
		}

		private static void SendMessage(object oCommand)
		{
			string s = (string)oCommand;
			NetworkStream stream = tcClient.GetStream();
			byte[] bytes = KDConvert.UTF8Encoder.GetBytes(s);
			stream.Write(bytes, 0, bytes.Length);
		}
		/*
		private static string GetASCIIMessage(string message)
		{
			foreach (string c in KDConvert.VietnameseToASCII.Keys)
			{
				message = message.Replace(c, KDConvert.VietnameseToASCII[c]);
			}
			return message;
		}
		*/
	}
}
