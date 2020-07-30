using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using Newtonsoft.Json;

namespace KDLib
{
	public class Obstacle
	{
        #region PrivateMembers
        #endregion
        
		#region PublicPRoperties
        [JsonIgnore]
		public int ID
		{
			get
			{
				return obstacleQuestion.ID;
			}
		}

		[JsonIgnore]
		public int CharCount
		{
			get
			{
				return obstacleQuestion.CharCount;
			}
		}

		[JsonIgnore]
		public string Content
		{
			get
			{
				return obstacleQuestion.Content;
			}
		}

		[JsonIgnore]
		public ImageSource Image
		{
			get
			{
				return obstacleQuestion.Image;
			}
		}

		[JsonIgnore]
		public Bitmap BitmapImage
		{
			get
			{
				return obstacleQuestion.BitmapImage;
			}
		}
        [JsonProperty]
        public ObstacleQuestion obstacleQuestion { get; set; }
        [JsonProperty]
        public List<ObstacleRowQuestion> RowList { get; set; }
        #endregion
        public Obstacle()
		{
			RowList = new List<ObstacleRowQuestion>();
		}
	}
}
