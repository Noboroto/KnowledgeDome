using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using Newtonsoft.Json;

namespace KDLib
{
	public class Obstacle
	{
		private ObstacleQuestion _obstacleQuestion;

		private List<ObstacleRowQuestion> _RowList;

		[JsonIgnore]
		public int ID
		{
			get
			{
				return _obstacleQuestion.ID;
			}
		}

		[JsonIgnore]
		public int CharCount
		{
			get
			{
				return _obstacleQuestion.CharCount;
			}
		}

		[JsonIgnore]
		public string Content
		{
			get
			{
				return _obstacleQuestion.Content;
			}
		}

		[JsonIgnore]
		public ImageSource Image
		{
			get
			{
				return _obstacleQuestion.Image;
			}
		}

		[JsonIgnore]
		public Bitmap BitmapImage
		{
			get
			{
				return _obstacleQuestion.BitmapImage;
			}
		}
		[JsonProperty]
		public ObstacleQuestion obstacleQuestion
		{
			get
			{
				return _obstacleQuestion;
			}
			set
			{
				_obstacleQuestion = value;
			}
		}
		[JsonProperty]
		public List<ObstacleRowQuestion> RowList
        {
			get
            {
				return _RowList;
            }
			set
            {
				_RowList = value;
            }
        }

		public Obstacle()
		{
			RowList = new List<ObstacleRowQuestion>();
		}

		/// <summary>
		/// Returns JSON string
		/// </summary>
		/// <returns></returns>

		public string CovertToJson ()
        {
			string s = "\"obstacleQuestion\":" + obstacleQuestion.ConvertToJson();
			s += ",\"RowList\":" + JsonConvert.SerializeObject(RowList);
			return s;
        }

		public static int Comparer(Obstacle a, Obstacle b)
		{
			return ObstacleQuestion.Comparer(a.obstacleQuestion, b.obstacleQuestion);
		}
	}
}
