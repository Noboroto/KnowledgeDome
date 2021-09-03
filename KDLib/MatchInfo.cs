using GalaSoft.MvvmLight;

using Newtonsoft.Json;

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
	}
}
