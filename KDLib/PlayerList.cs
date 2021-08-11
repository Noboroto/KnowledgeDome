using Newtonsoft.Json;

using System.Linq;

namespace KDLib
{
	public class PlayerList : KDCollectionBase<Player>
	{
		[JsonConstructor]
		public PlayerList()
		{
		}

		public override bool Contains(object id)
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
	}
}
