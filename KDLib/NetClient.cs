using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace KDLib
{
    public static class NetClient
    {
		private static TcpClient tcClient = new TcpClient();

		private static Thread tListenFromServer;

		private static Thread tSendMessage;

		public static void Connect(int ClientID, IPAddress ipaServerAddress)
		{
			try
			{
				tcClient.Connect(ipaServerAddress, Data.PortForTCP);
				NetworkStream stream = tcClient.GetStream();
				byte[] bytes = KDConvert.UTF8Encoder.GetBytes(ClientID.ToString());
				stream.Write(bytes, 0, bytes.Length);
				Thread_ListenFromServer();
			}
			catch (Exception e)
            {
				throw e;
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
