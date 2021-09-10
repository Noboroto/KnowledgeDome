using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using KDLib;

using System.Windows;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private MatchInfo _CurrentMatch;
        private MatchInfoList _matches;
        private int _SelectedMatchIndex;

        public MatchInfoList matches
		{
            get => _matches;
            set => Set(ref _matches, value);
		}
        public MatchInfo CurrentMatch
        {
            get => _CurrentMatch;
            set
            {
                Set(ref _CurrentMatch, value);
            }
        }
        public int SelectedMatchIndex
        {
            get => _SelectedMatchIndex;
            set
            {
                Set(ref _SelectedMatchIndex, value);
                Data.CurrentMatchIndex = value;
            }
        }

        public ICommand ChangeSource { get; }
        public MainPageViewModel()
        {
            Data.Initialize();
            SelectedMatchIndex = 0;
            matches = Data.MatchInfos;
            CurrentMatch = Data.MatchInfos[0];
            ChangeSource = new RelayCommand(() =>
            {
                CurrentMatch = Data.MatchInfos[1];
            });
        }
    }
}
