using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public class FinishQuestion : Question
	{
		#region PrivateMembers
		private int _Value;
		#endregion

		#region PublicProperties
		public int Value
		{
			get
			{
				return _Value;
			}
			set
			{
				Set(nameof(Value), ref _Value, value);
			}
		}
		#endregion

		[JsonConstructor]
		public FinishQuestion(int id, int value, string content, string answer)
			: base(id, content, answer)
		{
			Value = value;
		}
		public static FinishQuestion FromJson(string source)
		{
			return JsonConvert.DeserializeObject<FinishQuestion>(source);
		}

		public static FinishQuestion ReadFromFile(string path = @"Tests\FinishQuestion.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\FinishQuestion.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
