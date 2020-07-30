using Newtonsoft.Json;

namespace KDLib
{
	public class ObstacleRowQuestion : Question
	{
		#region PrivateMembers
		private int _CharCount;
        #endregion

        #region PublicProperties
        public int ObstacleParentID { get; set; }

        public int CharCount
        {
			get
            {
				return _CharCount;
            }
			set
            {
				_CharCount = value;
				NotifyPropertyChanged();
            }
        }
        #endregion

        [JsonConstructor]
		public ObstacleRowQuestion(int id, int parentID, int charcount, string content, string answer)
			: base(id, content, answer)
		{
			ObstacleParentID = parentID;
			CharCount = charcount;
		}
	}
}
