using Newtonsoft.Json;

using System.IO;

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


		public static FinishQuestionList ReadFromFile(string path = @"Tests\FinishQuestionList.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\FinishQuestionList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
