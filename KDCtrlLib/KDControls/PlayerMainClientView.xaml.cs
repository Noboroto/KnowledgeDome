using KDLib;

using System.Windows.Controls;
using System.Windows.Media;

namespace KDCtrlLib.KDControls
{
	/// <summary>
	/// Interaction logic for PlayerMainClientView.xaml
	/// </summary>
	public partial class PlayerMainClientView : UserControl
	{
		public Player PlayerData
		{
			get => (Player)DataContext;
			set => DataContext = value;
		}
		public ImageSource Source => ((Player)DataContext).Avatar;

		public PlayerMainClientView()
		{
			InitializeComponent();
		}
	}
}
