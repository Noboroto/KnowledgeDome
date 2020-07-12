using Newtonsoft.Json;

namespace KDLib
{
	public class FinishQuestion : Question
	{
		private int _Value;

		public int Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
				OnPropertyChanged("Value");
			}
		}

		public FinishQuestion(int value, string content, string answer)
			: this(Data.GenerateID(), value, content, answer)
		{
		}

		[JsonConstructor]
		public FinishQuestion(int id, int value, string content, string answer)
			: base(id, content, answer)
		{
			Value = value;
		}

		public void GetValueFrom(FinishQuestion question)
		{
			Value = question.Value;
			base.Content = question.Content;
			base.Answer = question.Answer;
		}

		public static int ValueComparer(FinishQuestion a, FinishQuestion b)
		{
			if (a.Value == b.Value)
			{
				return 0;
			}
			if (a.Value < b.Value)
			{
				return -1;
			}
			return 1;
		}
	}
}
