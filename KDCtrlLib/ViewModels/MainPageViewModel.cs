using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDCtrlLib.MessageForUI;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private MatchInfo _CurrentMatch;
        public ObservableCollection<string> IPs { get; set; }
        public string SelectedIP
		{
            get => Data.ChooseIP;
            set
            {               
                Data.ChooseIP = value;
                RaisePropertyChanged(nameof(SelectedIP));
                Messenger.Default.Send(new LoggingMessage(KDLogger.Info($"{nameof(SelectedIP)}: {SelectedIP}")));
            }
		}
        public MatchInfoList matches => Data.MatchInfos;
        public MatchInfo CurrentMatch
        {
            get => _CurrentMatch;
            set
            {
                Set(ref _CurrentMatch, value);
                Messenger.Default.Send(new LoggingMessage(KDLogger.Info($"SelectedMatch: {CurrentMatch.Name}")));
            }
        }
        public int SelectedMatchIndex
        {
            get => Data.CurrentMatchIndex;
            set
            {                
                Data.CurrentMatchIndex = value;
                RaisePropertyChanged(nameof(SelectedMatchIndex));
            }
        }

		#region Command
        public ICommand StartRoundCmd { get; set; }
        public ICommand ObstacleRoundCmd { get; set; }
        public ICommand SettingCmd { get; set; }
        public ICommand AccelerateCmd { get; set; }
        public ICommand FinishCmd { get; set; }
        public ICommand ExtraCmd { get; set; }
		#endregion

		public MainPageViewModel()
        {
            IPs = new ObservableCollection<string>(NetServer.GetLocalIPAddress());
            SelectedMatchIndex = 0;
            CurrentMatch = Data.MatchInfos[0];
        }
    }
}
