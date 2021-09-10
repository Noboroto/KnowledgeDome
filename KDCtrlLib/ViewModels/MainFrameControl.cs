using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using KDLib;
using KDLib.MessageForUI;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KDCtrlLib.ViewModels
{
	public class MainFrameControl : ViewModelBase
	{
		#region Private Properties
		private Uri _FrameSource;
		private string _WaitingSend;
		private Visibility _ChattingVisibility;
		private int _FrameColumnSpan;
		private Stack<Tuple<Uri, ProgramState>> NavigationStack;
		#endregion

		#region Public Properties
		public ObservableCollection<LogViewerInfo> LogsView { get; set; }
		public ObservableCollection<LogViewerInfo> Chatting { get; set; }
		public int FrameColumnSpan
		{
			get => _FrameColumnSpan;
			set => Set(ref _FrameColumnSpan, value);
		}
		public string WaitingSend
		{
			get => _WaitingSend;
			set => Set(ref _WaitingSend, value);
		}
		public Uri FrameSource
		{
			get => _FrameSource;
			set => Set(ref _FrameSource, value);
		}
		public Visibility ChattingVisibility
		{
			get => _ChattingVisibility;
			set => Set(ref _ChattingVisibility, value);
		}
		public ProgramState Status
		{
			get => Data.Status;
			set
			{
				Data.Status = value;
				RaisePropertyChanged(nameof(Status));
				RaisePropertyChanged(nameof(IsIdling));
				RaisePropertyChanged(nameof(IsPlaying));
				RaisePropertyChanged(nameof(IsPending));
				RaisePropertyChanged(nameof(IsEnded));
			}
		}
		public int CurrentRound
		{
			get => Data.CurrentRound;
			set
			{
				Data.CurrentRound = value;
				RaisePropertyChanged(nameof(CurrentRound));
				RaisePropertyChanged(nameof(IsExtra));
			}
		}
		public bool IsIdling => Data.Status == ProgramState.Idling;
		public bool IsPlaying => Data.Status == ProgramState.Playing;
		public bool IsPending => Data.Status == ProgramState.Pending;
		public bool IsEnded => Data.Status == ProgramState.Ended;
		public bool IsExtra => Data.CurrentRound == 5;
		#endregion

		#region Commands
		public ICommand SendCmd { get; set; }
		public ICommand StartRoundCmd { get; set; }
		public ICommand ObstacleRoundCmd { get; set; }
		public ICommand AcceblerationRoundCmd { get; set; }
		public ICommand FinishRoundCmd { get; set; }
		public ICommand ExtraRoundCmd { get; set; }
		public ICommand SettingCmd { get; set; }
		public ICommand GoBackCnd { get; set; }
		public ICommand ResultCmd { get; set; }
		public ICommand StartExtraCmd { get; set; }
		#endregion

		public MainFrameControl()
		{

			LogsView = new ObservableCollection<LogViewerInfo>();
			Chatting = new ObservableCollection<LogViewerInfo>();
			NavigationStack = new Stack<Tuple<Uri, ProgramState>>();

			Messenger.Default.Register<NavigateToMessage>(this, t => NavigateTo(t));
			Messenger.Default.Register<ChangeState>(this, t => UpdateState(t));
			ChattingVisibility = Visibility.Collapsed;
			FrameColumnSpan = 3;
			CurrentRound = 0;
			Status = ProgramState.Idling;

			CommandChecker();

			#region Server
			if (Data.ThisMacineType == Machine.Server)
			{
				Messenger.Default.Send(new NavigateToMessage(@"ServerView/MainServerPage.xaml"));
				Messenger.Default.Register<LogMess>(this, t => AddLog(t));
				Messenger.Default.Send(new LogMess(KDLogger.Info("Start")));
			}
			#endregion

			#region Client
			if (Data.ThisMacineType != Machine.Server)
			{
				Messenger.Default.Send(new NavigateToMessage(@"ConnectPage.xaml"));
			}
			#endregion

			#region Command
			SendCmd = new RelayCommand<KeyEventArgs>(e =>
			{
				if (e.Key == Key.Enter)
				{
					if (string.IsNullOrEmpty(WaitingSend.Trim())) return;
					if (Data.ThisMacineType == Machine.MC)
					{
						NetClient.SendCommand(new KDCommand(CommandType.MCToServer, WaitingSend));
					}
					else
					{
						Chatting.Add(KDLogger.ServerChat(WaitingSend));
						var command = new KDCommand(CommandType.ServerToMC, WaitingSend);
						foreach (var x in NetServer.MCAvailable)
						{
							NetServer.SendCommandToOne(x.Client, command);
						}
					}
					WaitingSend = "";
				}
			});
			StartRoundCmd = new RelayCommand(() =>
			{
				Status = ProgramState.Pending;
				CurrentRound = 1;
			});
			ObstacleRoundCmd = new RelayCommand(() =>
			{
				Status = ProgramState.Pending;
				CurrentRound = 2;
			});
			AcceblerationRoundCmd = new RelayCommand(() =>
			{
				Status = ProgramState.Pending;
				CurrentRound = 3;
			});
			FinishRoundCmd = new RelayCommand(() =>
			{
				Status = ProgramState.Pending;
				CurrentRound = 4;
			});
			ExtraRoundCmd = new RelayCommand(() =>
			{
				Status = ProgramState.Pending;
				CurrentRound = 5;
			});
			StartExtraCmd = new RelayCommand(() =>
			{

			});
			GoBackCnd = new RelayCommand(() =>
			{
				NetServer.SendCommandToAll(new KDCommand(CommandType.NavigateToRound, "0"));
				GoBack();
			});
			#endregion
		}

		private void CommandChecker()
		{
			Task.Run(() =>
			{
				while (true)
				{
					if (Data.FrameCommands.Count > 0)
					{
						KDCommand command = Data.FrameCommands.Peek();
						switch (command.PrefixCmd)
						{
							case CommandType.ChoosePlayer:
								Data.CurrentPlayerIndex = int.Parse(command.Content);
								goto EndCommand;
							case CommandType.NavigateToRound:
								Status = ProgramState.Playing;
								switch (int.Parse(command.Content))
								{
									case 0:
										GoBack();
										goto EndCommand;
									case 1:
										switch (Data.ThisMacineType)
										{
											case Machine.MC:
												Messenger.Default.Send(new NavigateToMessage(@"MCView\StartRoundMCView.xaml"));
												goto EndCommand;
											case Machine.Player:
											case Machine.Viewer:
												Messenger.Default.Send(new NavigateToMessage(@"StartRoundViewerPlayerPage.xaml"));
												goto EndCommand;
										}
										goto EndCommand;
									case 2:

										goto EndCommand;
									case 3:

										goto EndCommand;

								}
								goto EndCommand;
							case CommandType.EditScore:
								Player data = Data.FromJosn<Player>(command.Content);
								foreach (var x in Data.CurrentMatch.Players)
								{
									if (x.ID == data.ID)
									{
										x.Score = data.Score;
										goto EndCommand;
									}
								}
								goto EndCommand;
							case CommandType.MCToServer:
								Application.Current.Dispatcher.Invoke(() => Chatting.Add(KDLogger.MCChat(command.Content, $"{command.OwnIP};{command.Pos}")));
								var NewCommand = new KDCommand(CommandType.MCToMC, command.OwnIP, command.Pos, command.Content);
								foreach (var x in NetServer.MCAvailable)
								{
									NetServer.SendCommandToOne(x.Client, NewCommand);
								}
								goto EndCommand;
							case CommandType.ServerToMC:
								Application.Current.Dispatcher.Invoke(() => Chatting.Add(new LogViewerInfo(LogType.Server, command.Content)));
								goto EndCommand;
							case CommandType.MCToMC:
								Application.Current.Dispatcher.Invoke(() => Chatting.Add(new LogViewerInfo(LogType.MC, command.Content, $"{command.OwnIP};{command.Pos}")));
								goto EndCommand;
							EndCommand:
								if (Data.FrameCommands.Count > 0) Data.FrameCommands.Dequeue();
								continue;
							default:
								continue;
						}
					}
				}
			});
		}

		private void AddLog(LogMess m)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				LogsView.Add(m.Message);
			});
		}

		private void UpdateState(ChangeState m)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				Status = m.State;
			});
		}

		private void GoBack()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var x = NavigationStack.Pop();
				Messenger.Default.Send(new ChangeState(x.Item2));
				FrameSource = x.Item1;
			});

		}

		private void NavigateTo(NavigateToMessage m)
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (Data.ThisMacineType == Machine.MC && ChattingVisibility == Visibility.Collapsed)
				{
					ChattingVisibility = Visibility.Visible;
					FrameColumnSpan = 1;
				}
				ViewModelLocator.ReloadRound();
				NavigationStack.Push(new Tuple<Uri, ProgramState>(FrameSource, Status));
				FrameSource = m.Target;
			});
		}
	}
}