using Newtonsoft.Json;
using System.Collections.Specialized;
using System.ComponentModel;

namespace KDLib
{
	public class PlayerList : KDCollectionBase<Player>
	{
		public override void Add(Player item)
		{
			base.Add(item);
			item.PropertyChanged += Item_PropertyChanged;
		}

		public override void Remove(Player item)
		{
			item.PropertyChanged -= Item_PropertyChanged;
			base.Remove(item);
		}

		[JsonConstructor]
		public PlayerList()
		{
		}

		public override bool Contains(object id)
		{
			using (Enumerator enumerator = GetEnumerator())
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

		public Player FindFromName (string name)
        {
			return Find((Player p) => p.Name == name);
        }

		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
