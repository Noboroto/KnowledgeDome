using Newtonsoft.Json;

namespace KDLib
{
	public class ObstacleRowQuestion : Question
	{
        public int ObstacleParentID { get; set; }

        public int CharCount { get; set; }

        [JsonConstructor]
		public ObstacleRowQuestion(int id, int parentID, int charcount, string content, string answer)
			: base(id, content, answer)
		{
			ObstacleParentID = parentID;
			CharCount = charcount;
		}
	}
}
