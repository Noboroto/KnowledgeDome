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
