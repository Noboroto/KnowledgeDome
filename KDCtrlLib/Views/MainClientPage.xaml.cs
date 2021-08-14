using System.Collections.Generic;
using System.Windows.Controls;

using KDLib;
using KDCtrlLib.KDControl;

namespace KDCtrlLib.Views
{
	/// <summary>
	/// Interaction logic for MainClientPage.xaml
	/// </summary>
	public partial class MainClientPage : Page
	{
		public MainClientPage()
		{
			InitializeComponent();
			#region DEBUG DATA
			var b = new Player(12, "asdad");
			Data.Matches = new MatchList();
			Data.Matches.Add( new Match());
			Data.Matches[0].Players = new PlayerList { b, b, b, b };
			#endregion

			var show = new List<PlayerView> { first, second, third, fourth, fifth };
			int c = (show.Count < Data.Matches[0].Players.Count) ? show.Count : Data.Matches[0].Players.Count;
			for (int i = 0; i < c; ++i)
			{
				Data.Matches[0].Players[i].BackgroundColor = show[i].Background;
				show[i].PlayerData = Data.Matches[0].Players[i];
				show[i].Visibility = System.Windows.Visibility.Visible;
			}
		}
	}
}
