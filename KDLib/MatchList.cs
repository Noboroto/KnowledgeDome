using Newtonsoft.Json;

namespace KDLib
{
	public class MatchList : KDCollectionBase<Match>
	{
		public MatchList()
		{
		}

		public override bool Contains(object id)
		{
			return false;
		}

		public static MatchList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<MatchList>(source);
		}
		public override string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
