using KDCtrlLib.KDControls;

using KDLib;

using System.Collections.Generic;
using System.Windows.Controls;

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
            Data.Matches = new MatchList
            {
                new Match()
            };
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
