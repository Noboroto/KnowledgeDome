using Newtonsoft.Json;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class Player : KDObjectBase
	{
		private int _Hashcode;

		private string _Name;

		private byte[] _Avatar;

		private int _Score;

		public int Hashcode
		{
			get
			{
				return _Hashcode;
			}
			set
			{
				_Hashcode = value;
			}
		}

		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
				OnPropertyChanged("Name");
			}
		}

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
				File.WriteAllBytes("Images/Players/" + Hashcode.ToString() + ".png", RawAvatar);
				OnPropertyChanged("Avatar");
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

		public int Score
		{
			get
			{
				return _Score;
			}
			set
			{
				_Score = value;
				OnPropertyChanged("Score");
			}
		}

		private Player()
		{
			_Avatar = new byte[0];
		}

		public Player(string name, byte[] avatar)
			: this()
		{
			Hashcode = Data.GenerateID();
			Name = name;
			RawAvatar = avatar;
		}
		[JsonConstructor]
		public Player(int hashcode, string name)
		{
			Hashcode = hashcode;
			Name = name;
			if (File.Exists("Images\\Players\\" + Hashcode.ToString() + ".png")) RawAvatar = File.ReadAllBytes("Images\\Players\\" + Hashcode.ToString() + ".png");
		}

		public void GetValueFrom(Player p)
		{
			Name = p.Name;
			RawAvatar = p.RawAvatar;
		}
	}
}
