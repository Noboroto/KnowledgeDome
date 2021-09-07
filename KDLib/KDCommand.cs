using Newtonsoft.Json;

using System.Net;

namespace KDLib
{
	public class KDCommand
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties
		public Machine Machine { get; set; }

		public CommandType PrefixCmd { get; set; }

		public string Content { get; set; }

		public string IP { get; set; }

		[JsonIgnore]
		public IPAddress OwnIP
		{
			get
			{
				return IPAddress.Parse(IP);
			}
		}

		#endregion
		public KDCommand(CommandType prefix, string cmd = "")
		: this(Data.ThisMacineType, prefix, Data.ChooseIP, cmd)
		{

		}
		public KDCommand(CommandType prefix, IPEndPoint local, string cmd = "")
		: this(Data.ThisMacineType, prefix, local.Address.ToString(),  cmd)
		{
		}

		[JsonConstructor]
		public KDCommand(Machine type, CommandType prefix, string localIP, string cmd = "")
		{
			Machine = type;
			PrefixCmd = prefix;
			Content = cmd;
			IP = localIP;
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static KDCommand FromJson(string source)
		{
			if (string.IsNullOrEmpty(source)) return null;
			return JsonConvert.DeserializeObject<KDCommand>(source);
		}
	}
}
