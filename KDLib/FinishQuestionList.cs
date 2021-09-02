using Newtonsoft.Json;

namespace KDLib
{
	public class FinishQuestionList : KDCollectionBase<FinishQuestion>
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties
		#endregion

		public FinishQuestionList()
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

		public override string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static FinishQuestionList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<FinishQuestionList>(source);
		}
	}
}
