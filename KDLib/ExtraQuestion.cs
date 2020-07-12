using Newtonsoft.Json;

namespace KDLib
{
	public class ExtraQuestion : Question
	{
		public ExtraQuestion()
			: this(Data.GenerateID(), "", "")
		{
		}

		[JsonConstructor]
		public ExtraQuestion(int id, string question, string answer)
			: base(id,question, answer)
		{
		}
	}
}
