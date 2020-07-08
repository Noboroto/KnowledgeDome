namespace KDLib
{
	public class ExtraQuestion : Question
	{
		public ExtraQuestion()
			: this(Data.GenerateID(), "", "")
		{
		}

		public ExtraQuestion(int id, string question, string answer)
			: base(id,question, answer)
		{
		}
	}
}
