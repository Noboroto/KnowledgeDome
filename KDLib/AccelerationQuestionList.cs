using Newtonsoft.Json;

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

		public static AccelerationQuestionList FromJson(string json)
		{
			return JsonConvert.DeserializeObject<AccelerationQuestionList>(json);
		}

		public static AccelerationQuestionList ReadFromFile(string path = @"Tests\AccelerationQuestionList.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\AccelerationQuestionList.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
