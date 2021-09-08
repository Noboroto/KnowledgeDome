using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDLib.MessageForUI;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;

namespace KDCtrlLib.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
		#region Private
		private MatchInfo _CurrentMatch;
        private CancellationTokenSource cancellation;
		#endregion

		#region Public
		public ObservableCollection<string> IPs { get; set; }
        public string SelectedIP
		{
            get => Data.ChooseIP;
            set
            {               
                Data.ChooseIP = value;
                RaisePropertyChanged(nameof(SelectedIP));
                Messenger.Default.Send(new LogMess(KDLogger.Info($"{nameof(SelectedIP)}: {SelectedIP}")));
            }
		}
        public MatchInfoList matches => Data.MatchInfos;
        public MatchInfo CurrentMatch
        {
            get => Data.CurrentMatch;
            set
            {
                RaisePropertyChanged(nameof(CurrentMatch));
                Messenger.Default.Send(new LogMess(KDLogger.Info($"SelectedMatch: {CurrentMatch.Name}")));
            }
        }
        public int SelectedMatchIndex
        {
            get => Data.CurrentMatchIndex;
            set
            {                
                Data.CurrentMatchIndex = value;
                if (Data.ThisMacineType == Machine.Server) NetServer.SendCommandToAll(new KDCommand(CommandType.ChangeMatchToID, value.ToString()));
                RaisePropertyChanged(nameof(SelectedMatchIndex));
                RaisePropertyChanged(nameof(CurrentMatch));
            }
        }
		#endregion

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
            cancellation = new CancellationTokenSource();
            IPs = new ObservableCollection<string>(NetServer.GetLocalIPAddress());
            SelectedMatchIndex = 0;
            ProcessCommand(cancellation.Token);
            CurrentMatch = Data.MatchInfos[0];
        }

        private void ProcessCommand(CancellationToken token)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (Data.Commands.Count > 0)
                    {
                        try
                        {
                            KDCommand command = Data.Commands.Peek();
                            switch (command.PrefixCmd)
                            {
                                case CommandType.ChangeMatchToID:
                                    SelectedMatchIndex = int.Parse(command.Content);
                                    goto EndCommand;
                                EndCommand:
                                    if (Data.Commands.Count > 0) Data.Commands.Dequeue();
                                    continue;
                                default:
                                    continue;
                            }
                        }
                        catch (NullReferenceException)
                        {
                            continue;
                        }
                        catch (InvalidOperationException)
                        {
                            continue;
                        }
                    }
                }
            }, token);
        }

    }
}
