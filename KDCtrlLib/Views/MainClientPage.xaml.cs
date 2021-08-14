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
			var b = new Player(12, "", "adsasd asdasdasdas");
			var show = new List<PlayerView> { first, second, third, fourth, fifth };
			for (int i = 0; i < show.Count - 1; ++i)
			{
				show[i].PlayerData = b;
				show[i].Visibility = System.Windows.Visibility.Visible;
			}
		}
	}
}
