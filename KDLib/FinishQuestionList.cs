using Newtonsoft.Json;

namespace KDLib
{
	public class FinishQuestionList : KDCollectionBase<FinishQuestion>
	{
		[JsonIgnore]
		public int P10Count => FindAll((FinishQuestion q) => q.Value == 10).Count;

		[JsonIgnore]
		public int P20Count => FindAll((FinishQuestion q) => q.Value == 20).Count;

		[JsonIgnore]
		public int P30Count => FindAll((FinishQuestion q) => q.Value == 30).Count;

		public FinishQuestionList()
		{
		}

		public override void Add(FinishQuestion item)
		{
			base.Add(item);
		}

		public override void Remove(FinishQuestion item)
		{
			base.Remove(item);
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
