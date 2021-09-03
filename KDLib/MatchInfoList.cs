using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public class MatchInfoList : KDCollectionBase<MatchInfo>
	{
		public MatchInfoList()
		{
		}

		public override bool Contains(object id)
		{
			return false;
		}

		public static MatchInfoList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<MatchInfoList>(source);
		}
		public override string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static MatchInfoList ReadFromFile(string path = @"Tests\MatchInfoList.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\MatchInfoList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
