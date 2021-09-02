using Newtonsoft.Json;

namespace KDLib
{
	public class ExtraQuestionList : KDCollectionBase<ExtraQuestion>
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties
		#endregion

		public ExtraQuestionList()
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

		public static ExtraQuestionList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<ExtraQuestionList>(source);
		}
	}
}
