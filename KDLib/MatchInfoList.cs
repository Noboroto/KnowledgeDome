using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

namespace KDLib
{
	public class MatchInfoList : KDCollectionBase<MatchInfo>
	{
		public MatchInfoList()
		{
		}

		public MatchInfoList(IList<MatchInfo> list) : base(list)
		{

		}

		public bool Contains(object id)
		{
			return false;
		}

		public static MatchInfoList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<MatchInfoList>(source);
		}
		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static MatchInfoList ReadFromFile(string path = @"Tests\MatchInfoList.json")
		{
			if (!File.Exists(path)) return new MatchInfoList();
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\MatchInfoList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
