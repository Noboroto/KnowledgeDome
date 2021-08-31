using KDLib;

using System.Windows.Controls;
using System.Windows.Media;

namespace KDCtrlLib.Views
{
	/// <summary>
	/// Interaction logic for StartRoundPlayerPage.xaml
	/// </summary>
	public partial class StartRoundPlayerPage : Page
	{
		public StartRoundPlayerPage()
		{
			InitializeComponent();
			var b = new Player(12, "asdasd")
			{
				BackgroundColor = (Brush)new BrushConverter().ConvertFromString(@"#16acea"),
				ForegroundColor = Brushes.Black
			};
			Avatar.PlayerData = b;
			Avatar.Background = b.BackgroundColor;
			Avatar.Foreground = b.ForegroundColor;
		}
	}
}
