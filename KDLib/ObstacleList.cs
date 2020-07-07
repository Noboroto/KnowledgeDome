using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace KDLib
{
	public class ObstacleList : KDCollectionBase<Obstacle>
	{
		public ObstacleList()
		{
		}

		public override void Add(Obstacle item)
		{
			base.Add(item);
			item.CollectionChanged += Item_CollectionChanged;
		}

		public override void Remove(Obstacle item)
		{
			item.CollectionChanged -= Item_CollectionChanged;
			base.Remove(item);
		}

		public override bool Contains(int ID)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == ID)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void Edit(ObstacleQuestion q1, ObstacleQuestion q2)
		{
			q1.GetValueFrom(q2);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public void Save()
		{
			//Thêm sau
			//File.WriteAllText("Tests\\Obstacle.etai", AIEncoder.GetCode(text), Encoding.UTF8);
		}

		public Obstacle SearchFromID(int id)
		{
			return Find((Obstacle oq) => oq.ID == id);
		}

		private void Item_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}
