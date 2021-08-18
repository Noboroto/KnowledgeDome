using KDLib;

using System.Windows.Controls;
using System.Windows.Media;

namespace KDCtrlLib.KDControl
{
	/// <summary>
	/// Interaction logic for PlayerMainServerView.xaml
	/// </summary>
	public partial class PlayerMainServerView : UserControl
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
		public PlayerMainServerView()
		{
			InitializeComponent();
		}
	}
}
