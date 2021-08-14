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
			var show = new List<PlayerView> { first, second, third, fourth, fifth };
			for (int i = 0; i < show.Count; ++i)
			{
				show[i].Visibility = System.Windows.Visibility.Visible;
			}
		}
	}
}
