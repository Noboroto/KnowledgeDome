using Newtonsoft.Json;

using System.IO;

namespace KDLib
{
	public class AccelerationQuestion : Question
	{
		#region PrivateMembers
		private string _ImageType;

		private int _NumberOfImage;
		#endregion

		#region PublicProperties
		[JsonIgnore]
		public ImageList HintImages { get; set; }

		public string ImageType
		{
			get
			{
				return _ImageType;
			}
			set
			{
				Set(nameof(ImageType), ref _ImageType, value);
			}
		}

		public int NumberOfImage
		{
			get
			{
				return _NumberOfImage;
			}
			set
			{
				Set(nameof(NumberOfImage), ref _NumberOfImage, value);
			}
		}
		#endregion

		[JsonConstructor]
		public AccelerationQuestion(int id, string imagetype, string content, string answer)
			: base(id, content, answer)
		{
			int num = 0;
			ImageType = imagetype;
			while (File.Exists("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + imagetype))
			{
				HintImages.Add(File.ReadAllBytes("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + "." + imagetype));
				num++;
			}
		}

		public static AccelerationQuestion FromJson(string source)
		{
			return JsonConvert.DeserializeObject<AccelerationQuestion>(source);
		}

		public static AccelerationQuestion ReadFromFile(string path = @"Tests\AccelerationQuestion.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\AccelerationQuestion.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
