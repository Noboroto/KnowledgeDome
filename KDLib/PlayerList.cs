using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KDLib
{
	public class PlayerList : KDCollectionBase<Player>
	{
		[JsonConstructor]
		public PlayerList()
		{
		}
		public PlayerList(IList<Player> list) : base(list)
		{
		}

		public new void Add(Player player)
		{
			int index = Count % 5;
			player.SetBackground(KDConvert.BackgroundPlayer(index));
			player.SetForeground(KDConvert.ForegroundPlayer(index));
			base.Add(player);
		}

		public bool Contains(object id)
		{
			using (var enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == (int)id)
					{
						return true;
					}
				}
			}
			return false;
		}

		public Player FindFromName(string name)
		{
			return this.Where((Player p) => p.Name == name).First();
		}

		public static PlayerList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<PlayerList>(source);
		}
		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
		public static PlayerList ReadFromFile(string path = @"Tests\PlayerList.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\PlayerList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
