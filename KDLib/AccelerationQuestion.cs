using Newtonsoft.Json;
using System.Drawing;
using System.IO;

namespace KDLib
{
	public class AccelerationQuestion : Question
	{

		private ImageList _HintImages;

		private string _ImageType;

		[JsonIgnore]
		public ImageList HintImages
		{
			get
			{
				return _HintImages;
			}
			set
			{
				_HintImages = value;
			}
		}

		public string ImageType
        {
            get
            {
				return _ImageType;
            }
			set
            {
				ImageType = value;
            }
        }

		public AccelerationQuestion(string content, string answer, ImageList hintimages)
			: base(Data.GenerateID(),content, answer)
		{
			HintImages = hintimages;
		}

		[JsonConstructor]
		public AccelerationQuestion(int id, string type, string content, string answer)
			: base(id, content, answer)
		{
			int num = 0;
			ImageType = type;
			while (File.Exists("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + ".png"))
			{
				HintImages.Add(File.ReadAllBytes("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + "." + type));
				num++;
			}
		}

		public void GetValueFrom(AccelerationQuestion question)
		{
			base.Content = question.Content;
			base.Answer = question.Answer;
			HintImages = question.HintImages;
		}

		public void RemoveImages()
		{
			for (int i = 0; i < HintImages.Count; i++)
			{
				File.Delete("Tests\\Images\\" + ID.ToString() + "^" + i.ToString() + ".png");
			}
		}

		public static int CountComparer(AccelerationQuestion q1, AccelerationQuestion q2)
		{
			if (q1.HintImages.Count == q2.HintImages.Count)
			{
				return 0;
			}
			if (q1.HintImages.Count < q2.HintImages.Count)
			{
				return -1;
			}
			return 1;
		}
	}
}
