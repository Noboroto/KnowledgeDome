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
		private Brush _BackgroundColor;
		private Brush _ForegroundColor;

		[JsonIgnore]
		public Brush BackgroundColor
		{
			get => _BackgroundColor;
			set => Set(ref _BackgroundColor, value);
		}
		[JsonIgnore]
		public Brush ForegroundColor
		{
			get => _ForegroundColor;
			set => Set(ref _ForegroundColor, value);
		}
		public int ID { get; set; }
		public string Name { get; set; }
		public string BackCode => (BackgroundColor != null) ? BackgroundColor.ToString().Replace("#FF", "#") : "";
		public string ForeCode => (BackgroundColor != null) ? ForegroundColor.ToString().Replace("#FF", "#") : "";

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

		public Player(int id, string name) : this(id, name, "", "")
		{

		}

		[JsonConstructor]
		public Player(int id, string name, string backcode, string forecode)
		{
			ID = id;
			Name = name;
			SetBackground(backcode);
			SetForeground(forecode);
			if (File.Exists("Images\\Players\\" + ID.ToString() + ".png")) RawAvatar = File.ReadAllBytes("Images\\Players\\" + ID.ToString() + ".png");
			else RawAvatar = File.ReadAllBytes("Images\\Players\\default.png");
		}

		public void SetBackground(Brush color)
		{
			BackgroundColor = color.Clone();
		}

		public void SetForeground(Brush color)
		{
			ForegroundColor = color.Clone();
		}
		public void SetBackground(string code)
		{
			if (string.IsNullOrEmpty(code)) return;
			if (code[0] != '#') code = "#" + code;
			BackgroundColor = (Brush) new BrushConverter().ConvertFromString(code);
		}

		public void SetForeground (string code)
		{
			if (string.IsNullOrEmpty(code)) return;
			if (code[0] != '#') code = "#" + code;
			ForegroundColor = (Brush)new BrushConverter().ConvertFromString(code);
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static Player FromJson(string source)
		{
			return JsonConvert.DeserializeObject<Player>(source);
		}
		public static Player ReadFromFile(string path = @"Tests\Player.json")
		{
			if (!File.Exists(path)) return null;
			return FromJson(File.ReadAllText(path));
		}

		public void WriteToFile(string path = @"Tests\Player.json")
		{
			File.WriteAllText(path, ToJson());
		}
	}
}
