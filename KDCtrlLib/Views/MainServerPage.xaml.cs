using KDCtrlLib.KDControl;

using KDLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KDCtrlLib.Views
{
	/// <summary>
	/// Interaction logic for MainServerPage.xaml
	/// </summary>
	public partial class MainServerPage : Page
	{
		public MainServerPage()
		{
			InitializeComponent();
			#region DEBUG DATA
			var b = new Player(12, "asdad");
			Data.Matches = new MatchList();
			Data.Matches.Add(new Match());
			Data.Matches[0].Players = new PlayerList { b, b, b, b };
			#endregion

			var show = new List<PlayerMainClientView> { first, second, third, fourth, fifth };
			int c = (show.Count < Data.Matches[0].Players.Count) ? show.Count : Data.Matches[0].Players.Count;
			for (int i = 0; i < c; ++i)
			{
				Data.Matches[0].Players[i].BackgroundColor = show[i].Background;
				Data.Matches[0].Players[i].ForegroundColor = show[i].Foreground;
				show[i].PlayerData = Data.Matches[0].Players[i];
				show[i].Visibility = System.Windows.Visibility.Visible;
			}
		}
	}
}
