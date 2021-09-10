using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDLib.MessageForUI;

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		#region Private
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
		public ICommand SelectPlayerCmd { get; set; }
		#endregion

		public MainPageViewModel()
		{
			cancellation = new CancellationTokenSource();
			IPs = new ObservableCollection<string>(NetServer.GetLocalIPAddress());
			SelectedMatchIndex = 0;

			Data.RoundCommnads = new KDCommandList(CommandChecker);

			CurrentMatch = Data.MatchInfos[0];
			SelectPlayerCmd = new RelayCommand<RoutedEventArgs>((e) =>
			{
				Data.CurrentPlayerIndex = (e.Source as ListBox).SelectedIndex;
				switch (Data.CurrentRound)
				{
					case 1:
						NetServer.SendCommandToAll(new KDCommand(CommandType.ChoosePlayer, Data.CurrentPlayerIndex.ToString()));
						NetServer.SendCommandToAll(new KDCommand(CommandType.NavigateToRound, "1"));
						Messenger.Default.Send(new NavigateToMessage(@"ServerView\StartRoundServerView.xaml"));
						break;
					case 4:
						break;
				}
			});
		}

		private void CommandChecker(KDCommand command)
		{
			Task.Run(() =>
			{
				switch (command.PrefixCmd)
				{
					case CommandType.ChangeMatchToID:
						SelectedMatchIndex = int.Parse(command.Content);
						break;
					default:
						break;
				}
			});
		}
	}
}
