using Newtonsoft.Json;

namespace KDLib
{
	public class FinishQuestion : Question
	{
        public int Value { get; set; }

        [JsonConstructor]
		public FinishQuestion(int id, int value, string content, string answer)
			: base(id, content, answer)
		{
			Value = value;
		}
	}
}
