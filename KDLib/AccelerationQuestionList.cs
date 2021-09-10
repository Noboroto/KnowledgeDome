using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

namespace KDLib
{
	public class AccelerationQuestionList : KDCollectionBase<AccelerationQuestion>
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties
		#endregion

		public AccelerationQuestionList()
		{

		}

		public AccelerationQuestionList(IList<AccelerationQuestion> list) :base (list)
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

		public static AccelerationQuestionList FromJson(string json)
		{
			return JsonConvert.DeserializeObject<AccelerationQuestionList>(json);
		}

		public static AccelerationQuestionList ReadFromFile(string path = @"Tests\AccelerationQuestionList.json")
		{
			if (!File.Exists(path)) return new AccelerationQuestionList();
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\AccelerationQuestionList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
