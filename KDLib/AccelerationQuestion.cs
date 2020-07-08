using System.IO;

namespace KDLib
{
	public class AccelerationQuestion : Question
	{

		private ImageList _HintImages;

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

		public AccelerationQuestion(string content, string answer, ImageList hintimages)
			: base(Data.GenerateID(),content, answer)
		{
			HintImages = hintimages;
		}

		public AccelerationQuestion(int id, string content, string answer)
			: base(id, content, answer)
		{
			int num = 0;
			while (File.Exists("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + ".png"))
			{
				HintImages.Add(File.ReadAllBytes("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + ".png"));
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
