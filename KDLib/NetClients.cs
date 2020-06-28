using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KDLib
{
	public static class NetClients
	{
		private static TcpClient tcClient = new TcpClient();

		private static ASCIIEncoding aeEncoding = new ASCIIEncoding();

		private static Thread tListenFromServer;

		private static Thread tSendMessage;

		private static List<DataProvider> _Commands = new List<DataProvider>();

		public static List<DataProvider> Commands => _Commands;

		public static bool Connect(int iID, IPAddress ipaServerAddress, int iPort)
		{
			try
			{
				tcClient.Connect(ipaServerAddress, iPort);
				NetworkStream stream = tcClient.GetStream();
				byte[] bytes = aeEncoding.GetBytes(iID.ToString());
				stream.Write(bytes, 0, bytes.Length);
				Thread_ListenFromServer();
				return true;
			}
			catch
			{
				return false;
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
			tSendMessage = new Thread(SendMessage);
			tSendMessage.IsBackground = true;
			tSendMessage.Start(GetCodedMessage(sCommand.ToUpper()));
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
			}
		}

		private static void SendMessage(object oCommand)
		{
			string s = (string)oCommand;
			NetworkStream stream = tcClient.GetStream();
			byte[] bytes = aeEncoding.GetBytes(s);
			stream.Write(bytes, 0, bytes.Length);
		}

		private static string GetCodedMessage(string sMessage)
		{
			foreach (string Key in ChangeStandard.VietnameseToASCII.Keys)
            {
				sMessage.Replace(Key, ChangeStandard.VietnameseToASCII[Key]);
            }
			return sMessage;
		}

		private static string GetEncodedMessage(string sMessage)
		{
			foreach (string Key in ChangeStandard.ASCIIToVietnamese.Keys)
			{
				sMessage.Replace(Key, ChangeStandard.ASCIIToVietnamese[Key]);
			}
			return sMessage;
		}
	}
}
