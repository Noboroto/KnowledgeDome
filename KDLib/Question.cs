using GalaSoft.MvvmLight;

namespace KDLib
{
	public abstract class Question : ObservableObject
	{
		#region PrivateMembers
		private string _Content;
		private string _Answer;
		#endregion

		#region PublicProperties
		public string Content
		{
			get
			{
				return _Content;
			}
			set
			{
				Set(nameof(Content), ref _Content, value);
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
				Set(nameof(Answer), ref _Answer, value);
			}
		}
		public int ID { get; set; }
		#endregion

		internal Question(int id, string content, string answer)
		{
			ID = id;
			Content = content;
			Answer = answer;
		}
	}
}
