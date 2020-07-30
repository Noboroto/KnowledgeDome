namespace KDLib
{
	public abstract class Question : KDObjectBase
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
				_Content = value;
				NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
