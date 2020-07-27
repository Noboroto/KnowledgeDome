using Newtonsoft.Json;
using System.Drawing;
using System.IO;

namespace KDLib
{
	public class AccelerationQuestion : Question
	{
        [JsonIgnore]
        public ImageList HintImages { get; set; }

        public string ImageType { get; set; }

		public int NumberOfImage { get; set; }

        [JsonConstructor]
		public AccelerationQuestion(int id, string type, string content, string answer)
			: base(id, content, answer)
		{
			int num = 0;
			ImageType = type;
			while (File.Exists("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + type))
			{
				HintImages.Add(File.ReadAllBytes("Tests\\Images\\" + ID.ToString() + "^" + num.ToString() + "." + type));
				num++;
			}
		}
	}
}
