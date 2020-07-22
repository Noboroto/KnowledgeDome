using System.Collections.Specialized;

namespace KDLib
{
	public class ObstacleList : KDCollectionBase<Obstacle>
	{
		public ObstacleList()
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
	}
}
