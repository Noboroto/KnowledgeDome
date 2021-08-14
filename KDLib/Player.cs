using GalaSoft.MvvmLight;

using Newtonsoft.Json;

using System.IO;
using System.Windows.Media;

namespace KDLib
{
	public class Player : ObservableObject
	{
		private byte[] _Avatar;
		private int _Score;
		[JsonIgnore]
		public Brush BackgroundColor { get; set; }
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
				File.WriteAllBytes("Images/Players/" + ID + ".png", RawAvatar);
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

		[JsonIgnore]
		public int Score
		{
			get
			{
				return _Score;
			}
			set
			{
				Set(nameof(Score), ref _Score, value);
			}
		}

		private Player()
		{
			_Avatar = new byte[0];
		}

		[JsonConstructor]
		public Player(int id, string name)
		{
			ID = id;
			Name = name;
			if (File.Exists("Images\\Players\\" + ID.ToString() + ".png")) RawAvatar = File.ReadAllBytes("Images\\Players\\" + ID.ToString() + ".png");
			else RawAvatar = File.ReadAllBytes("Images\\Players\\default.png");
		}
	}
}
