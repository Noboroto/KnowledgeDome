using Newtonsoft.Json;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class Player : KDObjectBase
	{
        private byte[] _Avatar;

        public string Username { get; set; }

		public int ID { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
		public byte[] RawAvatar
		{
			get
			{
				return _Avatar;
			}
			set
			{
				_Avatar = value;
				File.WriteAllBytes("Images/Players/" + Username + ".png", RawAvatar);
			}
		}

		[JsonIgnore]
		public ImageSource Avatar
		{
			get
			{
				if (_Avatar.Length == 0)
				{
					return null;
				}
				return (ImageSource)new ImageSourceConverter().ConvertFrom(_Avatar);
			}
		}

        public int Score { get; set; }

        private Player()
		{
			_Avatar = new byte[0];
		}

		[JsonConstructor]
		public Player(int id, string username, string name)
		{
			ID = id;
			Username = username;
			Name = name;
			if (File.Exists("Images\\Players\\" + Username.ToString() + ".png")) RawAvatar = File.ReadAllBytes("Images\\Players\\" + Username.ToString() + ".png");
		}
	}
}
