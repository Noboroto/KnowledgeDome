using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDLib.MessageForUI;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
	public class MainWindowControl : ViewModelBase
	{
		#region Private Properties
		private string _WaitingSend;
		private Visibility _ChattingVisibility;
		private int _FrameColumnSpan;
		private Stack<NavigateToMessage> NavigationStack = new Stack<NavigateToMessage>();
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
			get => Data.CurrentPage;
			set
			{
				Data.CurrentPage = value;
				RaisePropertyChanged(nameof(FrameSource));
			}
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
		public string SystemName => Data.Title;
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

		public MainWindowControl()
		{

			LogsView = new ObservableCollection<LogViewerInfo>();
			Chatting = new ObservableCollection<LogViewerInfo>();

			Messenger.Default.Register<NavigateToMessage>(this, t => NavigateTo(t));
			Messenger.Default.Register<ChangeState>(this, t => UpdateState(t));
			ChattingVisibility = Visibility.Collapsed;
			FrameColumnSpan = 3;
			CurrentRound = 0;
			Status = ProgramState.Idling;

			Data.FrameCommands = new KDCommandList(CommandChecker);

			#region Server
			if (Data.ThisMacineType == Machine.Server)
			{
				Messenger.Default.Send(new NavigateToMessage(0));
				Messenger.Default.Register<LogMess>(this, t => AddLog(t));
				Messenger.Default.Send(new LogMess(KDLogger.Info("Start")));
			}
			#endregion

			#region Client
			if (Data.ThisMacineType != Machine.Server)
			{
				Messenger.Default.Send(new NavigateToMessage(-2));
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
				Messenger.Default.Send(new NavigateToMessage(ProgramState.Pending));
				CurrentRound = 1;
			});
			ObstacleRoundCmd = new RelayCommand(() =>
			{
				Messenger.Default.Send(new NavigateToMessage(ProgramState.Pending));
				CurrentRound = 2;
			});
			AcceblerationRoundCmd = new RelayCommand(() =>
			{
				Messenger.Default.Send(new NavigateToMessage(ProgramState.Pending));
				CurrentRound = 3;
			});
			FinishRoundCmd = new RelayCommand(() =>
			{
				Messenger.Default.Send(new NavigateToMessage(ProgramState.Pending));
				CurrentRound = 4;
			});
			ExtraRoundCmd = new RelayCommand(() =>
			{
				Messenger.Default.Send(new NavigateToMessage(ProgramState.Pending));
				CurrentRound = 5;
			});
			StartExtraCmd = new RelayCommand(() =>
			{

			});
			GoBackCnd = new RelayCommand(() =>
			{
				GoBack();
			});
			#endregion
		}

		private void CommandChecker(KDCommand command)
		{
			Task.Run(() =>
			{
				switch (command.PrefixCmd)
				{
					case CommandType.AccpetConnect:
						RaisePropertyChanged(nameof(SystemName));
						break;
					case CommandType.ChoosePlayer:
						Data.CurrentPlayerIndex = int.Parse(command.Content);
						break;
					case CommandType.NavigateToRound:
						Messenger.Default.Send(Data.FromJosn<NavigateToMessage>(command.Content));
						break;
					case CommandType.EditScore:
						Player data = Data.FromJosn<Player>(command.Content);
						foreach (var x in Data.CurrentMatch.Players)
						{
							if (x.ID == data.ID)
							{
								x.Score = data.Score;
								break;
							}
						}
						break;
					case CommandType.MCToServer:
						Application.Current.Dispatcher.Invoke(() => Chatting.Add(KDLogger.MCChat(command.Content, $"{command.OwnIP};{command.Pos}")));
						var NewCommand = new KDCommand(CommandType.MCToMC, command.OwnIP, command.Pos, command.Content);
						foreach (var x in NetServer.MCAvailable)
						{
							NetServer.SendCommandToOne(x.Client, NewCommand);
						}
						break;
					case CommandType.ServerToMC:
						Application.Current.Dispatcher.Invoke(() => Chatting.Add(new LogViewerInfo(LogType.Server, command.Content)));
						break;
					case CommandType.MCToMC:
						Application.Current.Dispatcher.Invoke(() => Chatting.Add(new LogViewerInfo(LogType.MC, command.Content, $"{command.OwnIP};{command.Pos}")));
						break;
					default:
						break;
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
			Status = m.State;
		}

		private void GoBack()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				var x = (NavigationStack.Count > 1) ? NavigationStack.Pop() : NavigationStack.Peek();
				NetServer.SendCommandToAll(new KDCommand(CommandType.NavigateToRound, Data.ToJson(x)));
				Status = x.State;
				Data.CurrentRound = x.ID;
				FrameSource = KDConvert.FindUri(x.ID);
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
				if (FrameSource != null && Data.ThisMacineType == Machine.Server) 
					NavigationStack.Push(new NavigateToMessage(KDConvert.UrlStringToID(FrameSource.ToString()), Status));
				Status = m.State;
				FrameSource = KDConvert.FindUri(m.ID);
			});
		}
	}
}