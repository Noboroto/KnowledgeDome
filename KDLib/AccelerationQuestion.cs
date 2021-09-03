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
