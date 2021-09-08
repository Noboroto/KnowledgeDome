using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace KDLib
{
	public class ClientInfo
	{
		public TcpClient Client { get; private set; }
		public int Pos { get; set; }
		public string IP => (Client.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
		public bool Connected => Client.Connected;
		public void Close ()
		{
			Client.Close();
		}
		public NetworkStream GetStream()
		{
			return Client.GetStream();
		}
		public ClientInfo (TcpClient client, int pos)
		{
			Client = client;
			Pos = pos;
		}
	}
}
