namespace KDLib
{
	public abstract class Question : KDObjectBase
	{
        public string Content { get; set; }

        public string Answer { get; set; }
        public int ID { get; set; }

        internal Question(int id, string content, string answer)
		{
			ID = id;
			Content = content;
			Answer = answer;
		}
	}
}
