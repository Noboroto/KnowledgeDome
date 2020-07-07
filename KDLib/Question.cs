namespace KDLib
{
	public abstract class Question : KDObjectBase
	{
		private string _Content;

		private string _Answer;

		public string Content
		{
			get
			{
				return _Content;
			}
			set
			{
				_Content = value;
				OnPropertyChanged("Content");
			}
		}

		public string Answer
		{
			get
			{
				return _Answer;
			}
			set
			{
				_Answer = value;
				OnPropertyChanged("Answer");
			}
		}

		internal Question(string content, string answer)
		{
			Content = content;
			Answer = answer;
		}

		public static int ContentComparer(Question a, Question b)
		{
			return string.Compare(a.Content, b.Content);
		}

		public static int AnswerComparer(Question a, Question b)
		{
			return string.Compare(a.Answer, b.Answer);
		}
	}
}
