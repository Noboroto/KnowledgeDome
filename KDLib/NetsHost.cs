using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KDLib
{
	public static class NetHost
	{
		private static TcpListener ListenerCenter;

		private static TcpClient[] Clients;

		private static ASCIIEncoding Encoding;

		private static Thread AcceptClientThread;

		private static Thread[] ClientListenerThreads;

		private static Thread SendMessageThread;

		private static List<DataProvider> _Commands = new List<DataProvider>();

		private static int _ClientConnected;

		public static int ClientConnected
		{
			get
			{
				return _ClientConnected;
			}
			set
			{
				_ClientConnected = value;
			}
		}

		public static List<DataProvider> Commands
		{
			get
			{
				return _Commands;
			}
			private set
			{
				_Commands = value;
			}
		}

		public static void Start()
		{
			ListenerCenter = new TcpListener(IPAddress.Any, 2644);
			Clients = new TcpClient[10];
			Encoding = new ASCIIEncoding();
			ClientListenerThreads = new Thread[10];
			ListenerCenter.Start();
			StartAcceptClient();
		}

		private static void StartAcceptClient()
		{
			AcceptClientThread = new Thread(AcceptClient);
			AcceptClientThread.IsBackground = true;
			AcceptClientThread.Start();
		}

		private static void AcceptClient()
		{
			while (true)
			{
				if (ListenerCenter.Pending())
				{
					TcpClient tcpClient = new TcpClient();
					tcpClient = ListenerCenter.AcceptTcpClient();
					NetworkStream stream = tcpClient.GetStream();
					byte[] array = new byte[1024];
					int count = stream.Read(array, 0, 1024);
					int num = int.Parse(Encoding.GetString(array, 0, count));
					if (Clients[num] == null)
					{
						Clients[num] = tcpClient;
						StartListenFromClient(num);
						ClientConnected++;
					}
				}
			}
		}

		private static void StartListenFromClient(int nClient)
		{
			ClientListenerThreads[nClient] = new Thread(ListenFromClient);
			ClientListenerThreads[nClient].IsBackground = true;
			ClientListenerThreads[nClient].Start(nClient);
		}

		private static void ListenFromClient(object oPlayer)
		{
			int num = (int)oPlayer;
			NetworkStream stream = Clients[num].GetStream();
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

		public static void SendCommand(string command)
		{
			SendMessageThread = new Thread(SendMessage);
			SendMessageThread.IsBackground = true;
			SendMessageThread.Start(GetCodedMessage(command));
		}

		private static void SendMessage(object command)
		{
			string s = (string)command;
			for (int i = 0; i < 10; i++)
			{
				if (Clients[i] != null)
				{
					NetworkStream stream = Clients[i].GetStream();
					byte[] bytes = Encoding.GetBytes(s);
					stream.Write(bytes, 0, bytes.Length);
				}
			}
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
