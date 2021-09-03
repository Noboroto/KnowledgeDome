using Newtonsoft.Json;

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
	}
}
