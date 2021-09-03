using KDLib;

using System.Windows.Controls;
using System.Windows.Media;

namespace KDCtrlLib.KDControls
{
	/// <summary>
	/// Interaction logic for AvatarPlayerSingleView.xaml
	/// </summary>
	public partial class AvatarPlayerSingleView : UserControl
	{
		private Player _PlayerData;

		public Player PlayerData
		{
			get => _PlayerData;
			set
			{
				_PlayerData = value;
				DataContext = _PlayerData;
			}
		}
		public ImageSource Source => _PlayerData.Avatar;
		public AvatarPlayerSingleView()
		{
			InitializeComponent();
		}
	}
}
