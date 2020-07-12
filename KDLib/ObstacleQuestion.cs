using Newtonsoft.Json;
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
		[JsonProperty]
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
		[JsonIgnore]
		private byte[] RawImage
		{
			get
			{
				return _Image;
			}
			set
			{
				_Image = value;
				File.WriteAllBytes("Tests\\Images\\" + ID.ToString() + "." +  _ImageType, RawImage);
			}
		}

		[JsonIgnore]
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
		[JsonIgnore]
		public string ImageType
        {
			get
            {
				return _ImageType;
            }
			set
            {
				_ImageType = value;
            }
        }
		[JsonIgnore]
		public Bitmap BitmapImage => new Bitmap(new MemoryStream(RawImage));

		public void GetValueFrom(ObstacleQuestion q)
		{
			CharCount = q.CharCount;
			Content = q.Content;
			RawImage = q.RawImage;	
		}

		public ObstacleQuestion(int id, int charcount, string content)
			: base(id, content, "")
		{
			CharCount = charcount;
		}


		public string ConvertToJson()
		{
			string s = "{";
			s += "\"ID\":" + ID;
			s += ",\"ImageType\":" + '"' + ImageType + '"';
			s += ",\"CharCount\":" + CharCount;
			s += ",\"Content\":" + '"' + Content + '"';
			s += ",\"Answer\":" + '"' + Answer + '"';
			return s + "}";
		}

		public static int Comparer(ObstacleQuestion a, ObstacleQuestion b)
		{
			return string.Compare(a.Content, b.Content);
		}
	}
}