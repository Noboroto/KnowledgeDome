using KDLib;

using System.Windows.Controls;
using System.Windows.Media;

namespace KDCtrlLib.Views
{
	/// <summary>
	/// Interaction logic for StartRoundViewerPlayerPage.xaml
	/// </summary>
	public partial class StartRoundViewerPlayerPage : Page
	{
		public StartRoundViewerPlayerPage()
		{
			InitializeComponent();
			var b = new Player(12, "asdasd")
			{
				BackgroundColor = (Brush)new BrushConverter().ConvertFromString(@"#16acea"),
				ForegroundColor = Brushes.Black
			};
			Avatar.PlayerData = b;
		}
	}
}
