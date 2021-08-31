using KDLib;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace KDCtrlLib.KDControls
{
    /// <summary>
    /// Interaction logic for PlayerMainClientView.xaml
    /// </summary>
    public partial class PlayerMainClientView : UserControl
    {
        private Player _PlayerData;
        public static DependencyProperty ShowScoreProperty = DependencyProperty.Register(nameof(ShowScore), typeof(bool), typeof(PlayerMainClientView), new PropertyMetadata(true));

        public bool ShowScore
		{
            get => (bool)GetValue(ShowScoreProperty);
            set => SetValue(ShowScoreProperty, value);
		}

        Visibility ScoreVisibility => (ShowScore) ? Visibility.Visible : Visibility.Hidden;
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

        public PlayerMainClientView()
        {
            InitializeComponent();
        }
    }
}
