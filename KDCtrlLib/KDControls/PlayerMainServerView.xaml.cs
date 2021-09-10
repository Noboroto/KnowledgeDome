using KDLib;

using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace KDCtrlLib.KDControls
{
	/// <summary>
	/// Interaction logic for PlayerMainServerView.xaml
	/// </summary>
	public partial class PlayerMainServerView : UserControl
	{
		public Player PlayerData
		{
			get => (Player)DataContext;
			set => DataContext = value;
		}
		public ImageSource Source => ((Player)DataContext).Avatar;
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
					NetServer.SendCommandToAll(new KDCommand(CommandType.EditScore, PlayerData.ToJson()));
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
