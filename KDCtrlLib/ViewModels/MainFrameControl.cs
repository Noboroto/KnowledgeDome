using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using KDLib;
using KDLib.MessageForUI;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace KDCtrlLib.ViewModels
{
    public class MainFrameControl : ViewModelBase
    {
        #region Private Properties
        private Uri _FrameSource;
        private string _WaitingSend;
        private Visibility _ChattingVisibility;
        #endregion

        #region Public Properties
        public ObservableCollection<LogViewerInfo> LogsView { get; set; }
        public ObservableCollection<LogViewerInfo> Chatting { get; set; }
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

            #region Server
            if (Data.ThisMacineType == Machine.Server)
			{
                Messenger.Default.Send(new NavigateToMessage(@"ServerView/MainServerPage.xaml"));
                Messenger.Default.Register<LogMess>(this, t => AddLog(t));
                Messenger.Default.Send(new LogMess(KDLogger.Info("Start")));
                Messenger.Default.Send(new LogMess(KDLogger.Info("Start", LogType.Player)));
                Messenger.Default.Send(new LogMess(KDLogger.Info("Start", LogType.MC)));
                Messenger.Default.Send(new LogMess(KDLogger.Info("Start", LogType.Viewer)));
                Messenger.Default.Send(new LogMess(KDLogger.Info("Start", LogType.Warn)));
                Messenger.Default.Send(new LogMess(KDLogger.Info("Start", LogType.Error)));
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
                    if (Data.ThisMacineType == Machine.MC)
                    {
                        //không lưu log
                        Chatting.Add(new LogViewerInfo(LogType.MC, WaitingSend));
                    }
                    else
                    {
                        Chatting.Add(KDLogger.ServerChat(WaitingSend));
                    }
                    WaitingSend = "";
                }
            });
			#endregion
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
				}
                FrameSource = m.Target;
            });
		}
    }
}