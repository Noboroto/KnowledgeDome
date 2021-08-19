using KDLib;

using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace KDCtrlLib.kdcontrols
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

		private void DialogHost_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
		{
			try
			{
				if ((bool)eventArgs.Parameter)
				{
					PlayerData.Score = int.Parse(EditScore.Text);
				}
			}
			catch (ArgumentNullException)
			{
				PlayerData.Score = 0;
			}
			catch
			{
				return;
			}
			finally
			{				
				EditScore.Text = PlayerData.Score.ToString();
			}
			return;
		}
	}
}
