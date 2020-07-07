using System.Collections.Generic;
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

		public PlayerList()
		{
		}

		public PlayerList(string data, byte[] avatars)
		{
		}

		public override bool Contains(int hashcode)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Hashcode == hashcode)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
