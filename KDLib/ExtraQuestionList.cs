namespace KDLib
{
	public class ExtraQuestionList : KDCollectionBase<ExtraQuestion>
	{
		public ExtraQuestionList()
		{
		}

		public override bool Contains(int id)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == id)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Sửa sau
		/// </summary>
		public void Save()
		{
			/*string text = "";
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ExtraQuestion current = enumerator.Current;
					text += current.TextData();
				}
			}
			File.WriteAllText("Tests\\Extra.etai", AIEncoder.GetCode(text), Encoding.UTF8);*/
		}

		public ExtraQuestion FindFromID(int id)
		{
			return Find((ExtraQuestion sq) => sq.ID == id);
		}
	}
}
