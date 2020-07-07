using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class ObstacleQuestion : Question
	{
		private int _CharCount;

		private byte[] _Image;

		private string _ImageType;

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

		private byte[] RawImage
		{
			get
			{
				return _Image;
			}
			set
			{
				_Image = value;
				File.WriteAllBytes("Tests\\Images\\" + ID.ToString() + _ImageType, RawImage);
			}
		}

		public ImageSource Image
		{
			get
			{
				if (RawImage.Length != 0)
				{
					return (ImageSource)new ImageSourceConverter().ConvertFrom(RawImage);
				}
				return null;
			}
		}

		public Bitmap BitmapImage => new Bitmap(new MemoryStream(RawImage));

		public void GetValueFrom(ObstacleQuestion q)
		{
			CharCount = q.CharCount;
			Content = q.Content;
			RawImage = q.RawImage;
		}

		public ObstacleQuestion(int id, int charcount, string content)
			: base(id, "",content)
		{
			CharCount = charcount;
		}

		public static int Comparer(ObstacleQuestion a, ObstacleQuestion b)
		{
			return string.Compare(a.Content, b.Content);
		}
	}
}