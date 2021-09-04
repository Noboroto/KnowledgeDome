using GalaSoft.MvvmLight;

using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public class MatchInfo : ObservableObject
	{
		#region PrivateMembers
		private string _Name;
		private PlayerList _Players;
		#endregion

		#region PublicProperties

		public string Name
		{
			get => _Name;
			private set => Set(ref _Name, value);
		}

		public PlayerList Players
		{
			get => _Players;
			set => Set(ref _Players, value);
		}
		#endregion


		[JsonConstructor]
		public MatchInfo(string name, PlayerList players)
		{
			Name = name;
			Players = players;
		}

		public MatchInfo(string name = "No Name")
			: this(name, new PlayerList())
		{
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
