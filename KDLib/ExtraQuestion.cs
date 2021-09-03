using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public class ExtraQuestion : Question
	{
		#region PrivateMembers
		#endregion

		#region PublicProperties
		#endregion

		[JsonConstructor]
		public ExtraQuestion(int id, string question, string answer)
			: base(id, question, answer)
		{
		}

		public static ExtraQuestion FromJson(string source)
		{
			return JsonConvert.DeserializeObject<ExtraQuestion>(source);
		}

		public static ExtraQuestion ReadFromFile(string path = @"Tests\ExtraQuestion.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\ExtraQuestion.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
