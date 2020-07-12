using Newtonsoft.Json;

namespace KDLib
{
	public class ObstacleRowQuestion : Question
	{
		private int _CharCount;

		private int _ObstacleParentID;

		public int ObstacleParentID
        {
			get
			{
				return _ObstacleParentID;
			}
			set
			{
				_ObstacleParentID = value;
			}
		}

		public int CharCount
		{
			get
			{
				return _CharCount;
			}
			set
			{
				_CharCount = value;
			}
		}
		[JsonConstructor]
		public ObstacleRowQuestion(int id, int parentID, int charcount, string content, string answer)
			: base(id, content, answer)
		{
			ObstacleParentID = parentID;
			CharCount = charcount;
		}

		public void GetValueFrom(ObstacleRowQuestion question)
		{
			ObstacleParentID = question.ObstacleParentID;
			CharCount = question.CharCount;
			base.Content = question.Content;
			base.Answer = question.Answer;
		}
	}
}
