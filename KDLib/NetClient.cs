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

		private const int PortForTCP = 2644;

		private const int PortForChecker = 2645;

		public static void Connect(int ClientID, IPAddress ipaServerAddress)
		{
			try
			{
				tcClient.Connect(ipaServerAddress, PortForTCP);
				NetworkStream stream = tcClient.GetStream();
				byte[] bytes = KDConvert.ASCIIEncoder.GetBytes(ClientID.ToString());
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

		public static void SendCommand(string sCommand)
		{
			/*
			tSendMessage = new Thread(SendMessage);
			tSendMessage.IsBackground = true;
			tSendMessage.Start(GetCodedMessage(sCommand.ToUpper()));*/
		}

		private static void ListenFromServer()
		{
			NetworkStream stream = tcClient.GetStream();
			while (true)
			{
				/*byte[] array = new byte[1024];
				int num = 0;
				try
				{
					num = stream.Read(array, 0, 1024);
				}
				catch
				{
					DataProvider dataProvider = new DataProvider("[", "]", AIObjectBase.TagDefinition('w', "disconnect"), haskey: false);
					Commands.Add(dataProvider.Children[0]);
					return;
				}
				if (num != 0)
				{
					foreach (DataProvider child in new DataProvider("[", "]", GetEncodedMessage(aeEncoding.GetString(array, 0, num)), haskey: false).Children)
					{
						Commands.Add(child);
					}
					continue;
				}
				break;
				*/
			}
		}

		private static void SendMessage(object oCommand)
		{
			/*
			string s = (string)oCommand;
			NetworkStream stream = tcClient.GetStream();
			byte[] bytes = aeEncoding.GetBytes(s);
			stream.Write(bytes, 0, bytes.Length);*/
		}
	}
}
