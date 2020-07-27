using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class ImageList : KDCollectionBase<ImageSource>
	{
		private List<byte[]> RawImages;

		public ImageList()
		{
			RawImages = new List<byte[]>();
		}

		public ImageList(ImageList images)
			: this()
		{
			foreach (byte[] rawImage in images.RawImages)
			{
				Add(rawImage);
			}
		}

		public void Add(byte[] image)
		{
			RawImages.Add(image);
			Add((ImageSource)new ImageSourceConverter().ConvertFrom(image));
		}

        public override bool Contains(object id)
        {
			return true;
        }
	}
}
