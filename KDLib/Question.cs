namespace KDLib
{
	public abstract class Question : KDObjectBase
	{
		private int _ID;

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
		public int ID
        {
            get
            {
				return _ID;
            }
			set
            {
				_ID = value;
				OnPropertyChanged("ID");
            }
        }

		internal Question(int id, string content, string answer)
		{
			ID = id;
			Content = content;
			Answer = answer;
		}
		public static int IDComparer(Question a, Question b)
		{
			if (a.ID < b.ID) return -1;
			if (a.ID > b.ID) return 1;
			return 0;
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
