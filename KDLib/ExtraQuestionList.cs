namespace KDLib
{
	public class ExtraQuestionList : KDCollectionBase<ExtraQuestion>
	{
		public ExtraQuestionList()
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
