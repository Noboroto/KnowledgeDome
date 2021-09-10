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

		public int Pos { get; set; }

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
		: this(Data.ThisMacineType, prefix, Data.ChooseIP, Data.Pos, cmd)
		{

		}
		public KDCommand(CommandType prefix, IPAddress local, string cmd = "")
		: this(Data.ThisMacineType, prefix, local.ToString(), Data.Pos, cmd)
		{
		}

		public KDCommand(CommandType prefix, IPAddress local, int pos, string cmd = "")
		: this(Data.ThisMacineType, prefix, local.ToString(), pos, cmd)
		{
		}

		[JsonConstructor]
		public KDCommand(Machine type, CommandType prefix, string localIP, int pos, string cmd = "")
		{
			Machine = type;
			PrefixCmd = prefix;
			Content = cmd;
			IP = localIP;
			Pos = pos;
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
