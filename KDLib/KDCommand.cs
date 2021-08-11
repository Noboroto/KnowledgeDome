using Newtonsoft.Json;

using System.Net;

namespace KDLib
{
	public class KDCommand
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties
		public MachineType Machine { get; set; }

		public CommandType PrefixCmd { get; set; }

		public string Content { get; set; }

		public string IP { get; set; }

		public int Port { get; set; }

		[JsonIgnore]
		public IPEndPoint ID
		{
			get
			{
				return new IPEndPoint(IPAddress.Parse(IP), Port);
			}
		}

		#endregion

		public KDCommand(CommandType prefix, IPEndPoint local, string cmd = "")
		: this(Data.ThisMacineType, prefix, local.Address.ToString(), local.Port, cmd)
		{
		}

		[JsonConstructor]
		public KDCommand(MachineType type, CommandType prefix, string localIP, int localPort, string cmd = "")
		{
			Machine = type;
			PrefixCmd = prefix;
			Content = cmd;
			IP = localIP;
			Port = localPort;
		}
	}
}
