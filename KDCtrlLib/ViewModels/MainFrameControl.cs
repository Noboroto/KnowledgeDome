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

namespace KDCtrlLib.ViewModels
{
    public class MainFrameControl : ViewModelBase
    {
        #region Private Properties
        private Uri _FrameSource;
        private string _WaitingSend;
        private Visibility _ChattingVisibility;
        private int _FrameColumnSpan;
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
        #endregion

        #region Commands
        public ICommand SendCmd { get; set; }
		#endregion

		public MainFrameControl()
        {
            
            LogsView = new ObservableCollection<LogViewerInfo>();
            Chatting = new ObservableCollection<LogViewerInfo>();

            Messenger.Default.Register<NavigateToMessage>(this, t => NavigateTo(t));
            ChattingVisibility = Visibility.Collapsed;
            FrameColumnSpan = 3;
            ProcessCommand();

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
			#endregion
		}

        private void ProcessCommand()
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
            });
        }

        private void AddLog (LogMess m)
		{
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogsView.Add(m.Message);
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
                FrameSource = m.Target;
            });
		}
    }
}