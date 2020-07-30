using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class ObstacleQuestion : Question
	{
        #region PrivateMembers
        private byte[] _Image;
		private int _CharCount;
        #endregion

        #region PublicProperties
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
        #endregion

        [JsonConstructor]
		public ObstacleQuestion(int id, int charcount, string type, string content)
			: base(id, content, "")
		{
			ImageType = type;
			CharCount = charcount;
		}

		public void GetValueFrom(ObstacleQuestion q)
		{
			CharCount = q.CharCount;
			Content = q.Content;
			RawImage = q.RawImage;	
		}
		
		public static int Comparer(ObstacleQuestion a, ObstacleQuestion b)
		{
			return string.Compare(a.Content, b.Content);
		}
	}
}