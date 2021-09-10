using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

namespace KDLib
{
	public class ObstacleList : KDCollectionBase<Obstacle>
	{
		public ObstacleList()
		{

		}

		public ObstacleList(IList<Obstacle> list) : base(list)
		{

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

		public static ObstacleList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<ObstacleList>(source);
		}
		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static ObstacleList ReadFromFile(string path = @"Tests\ObstacleList.json")
		{
			if (!File.Exists(path)) return new ObstacleList();
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\ObstacleList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
