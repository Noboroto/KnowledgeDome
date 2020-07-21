using Newtonsoft.Json;
using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class Player : KDObjectBase
	{
		private string _ID;

		private string _Name;

		private byte[] _Avatar;

		private int _Score;

		public string ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID = value;
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
				File.WriteAllBytes("Images/Players/" + ID + ".png", RawAvatar);
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

		[JsonConstructor]
		public Player(string id, string name)
		{
			ID = id;
			Name = name;
			if (File.Exists("Images\\Players\\" + ID.ToString() + ".png")) RawAvatar = File.ReadAllBytes("Images\\Players\\" + ID.ToString() + ".png");
		}

		public void GetValueFrom(Player p)
		{
			Name = p.Name;
			RawAvatar = p.RawAvatar;
		}
	}
}
