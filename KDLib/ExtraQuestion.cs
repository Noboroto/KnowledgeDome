using Newtonsoft.Json;

namespace KDLib
{
	public class ExtraQuestion : Question
	{
		[JsonConstructor]
		public ExtraQuestion(int id, string question, string answer)
			: base(id,question, answer)
		{
		}
	}
}
