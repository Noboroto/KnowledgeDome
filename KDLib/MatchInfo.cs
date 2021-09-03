using GalaSoft.MvvmLight;

using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public class MatchInfo : ObservableObject
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties

		public string Name { get; private set; }

		public PlayerList Players { get; set; }
		#endregion

		public MatchInfo()
		{
			Players = new PlayerList();
		}

		[JsonConstructor]
		public MatchInfo(PlayerList players)
		{
			Players = players;
		}

		public MatchInfo(string name)
			: this()
		{
			Name = name;
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
		public static MatchInfo FromJson(string source)
		{
			return JsonConvert.DeserializeObject<MatchInfo>(source);
		}


		public static MatchInfo ReadFromFile(string path = @"Tests\MatchInfo.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\MatchInfo.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
