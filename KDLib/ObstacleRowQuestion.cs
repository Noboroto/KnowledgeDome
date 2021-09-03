using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public class ObstacleRowQuestion : Question
	{
		#region PrivateMembers
		private int _CharCount;
		#endregion

		#region PublicProperties
		public int ObstacleParentID { get; set; }

		public int CharCount
		{
			get
			{
				return _CharCount;
			}
			set
			{
				Set(nameof(CharCount), ref _CharCount, value);
			}
		}
		#endregion

		[JsonConstructor]
		public ObstacleRowQuestion(int id, int parentID, int charcount, string content, string answer)
			: base(id, content, answer)
		{
			ObstacleParentID = parentID;
			CharCount = charcount;
		}

		public static ObstacleRowQuestion FromJson(string source)
		{
			return JsonConvert.DeserializeObject<ObstacleRowQuestion>(source);
		}
		public static ObstacleRowQuestion ReadFromFile(string path = @"Tests\ObstacleRowQuestion.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\ObstacleRowQuestion.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
