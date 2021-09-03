using KDCtrlLib.KDControls;

using KDLib;

using System.Collections.Generic;
using System.Windows.Controls;

namespace KDCtrlLib.Views.ServerView
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
            Data.MatchInfos = new MatchInfoList
            {
                new MatchInfo()
            };
            Data.MatchInfos[0].Players = new PlayerList { b, b, b, b };
            #endregion

            var show = new List<PlayerMainServerView> { first, second, third, fourth, fifth };
            int c = (show.Count < Data.MatchInfos[0].Players.Count) ? show.Count : Data.MatchInfos[0].Players.Count;
            for (int i = 0; i < c; ++i)
            {
                Data.MatchInfos[0].Players[i].BackgroundColor = show[i].Background;
                Data.MatchInfos[0].Players[i].ForegroundColor = show[i].Foreground;
                show[i].PlayerData = Data.MatchInfos[0].Players[i];
                show[i].Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
