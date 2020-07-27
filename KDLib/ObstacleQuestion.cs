using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class ObstacleQuestion : Question
	{
        private byte[] _Image;

        [JsonProperty]
        public int CharCount { get; set; }
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
				File.WriteAllBytes("Tests\\Images\\" + ID.ToString() + "." +  ImageType, RawImage);
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

        public string ImageType { get; set; }
        [JsonIgnore]
		public Bitmap BitmapImage => new Bitmap(new MemoryStream(RawImage));

		public void GetValueFrom(ObstacleQuestion q)
		{
			CharCount = q.CharCount;
			Content = q.Content;
			RawImage = q.RawImage;	
		}

		[JsonConstructor]
		public ObstacleQuestion(int id, int charcount, string type, string content)
			: base(id, content, "")
		{
			ImageType = type;
			CharCount = charcount;
		}

		public static int Comparer(ObstacleQuestion a, ObstacleQuestion b)
		{
			return string.Compare(a.Content, b.Content);
		}
	}
}