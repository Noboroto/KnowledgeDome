using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

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
		public ExtraQuestionList(IList<ExtraQuestion> list) :base (list)
		{

		}
		public bool Contains(object id)
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

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static ExtraQuestionList FromJson(string source)
		{
			return JsonConvert.DeserializeObject<ExtraQuestionList>(source);
		}
		public static ExtraQuestionList ReadFromFile(string path = @"Tests\ExtraQuestionList.json")
		{
			if (!File.Exists(path)) return new ExtraQuestionList();
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\ExtraQuestionList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
