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

		public new void Remove(ImageSource source)
		{
			int index = IndexOf(source);
			RawImages.RemoveAt(index);
			RemoveAt(index);
		}

		public void Save(int ID, string ImageType)
		{
			for (int i = 0; i < base.Count; i++)
			{
				File.WriteAllBytes("Tests\\Images\\" + ID.ToString() + "^" + i.ToString() + "." + ImageType, RawImages[i]);
			}
		}
	}
}
