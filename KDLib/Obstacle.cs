using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class Obstacle : KDCollectionBase<ObstacleRowQuestion>
	{
		private ObstacleQuestion _obstacleQuestion;

		public int ID
		{
			get
			{
				return _obstacleQuestion.ID;
			}
		}

		public int CharCount
		{
			get
			{
				return _obstacleQuestion.CharCount;
			}
		}

		public string Content
		{
			get
			{
				return _obstacleQuestion.Answer;
			}
		}
		
		public ImageSource Image
        {
			get
            {
				return _obstacleQuestion.Image;
            }
        }
		
		public Bitmap BitmapImage
        {
			get
            {
				return _obstacleQuestion.BitmapImage;
            }
        }
		
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

		public Obstacle()
		{
		}
		public static int Comparer(Obstacle a, Obstacle b)
		{
			return ObstacleQuestion.Comparer(a.obstacleQuestion, b.obstacleQuestion);
		}
	}
}
