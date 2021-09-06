using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using KDLib;
using KDCtrlLib.MessageForUI;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;

namespace KDCtrlLib.ViewModels
{
    public class MainFrameControl : ViewModelBase
    {
        #region Private Properties
        private Uri _FrameSource;
        private string _WaitingSend;
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
        #endregion

        #region Commands
        public ICommand SendCmd { get; set; }
		#endregion

		public MainFrameControl()
        {
            
            LogsView = new ObservableCollection<LogViewerInfo>();
            Chatting = new ObservableCollection<LogViewerInfo>();

            Messenger.Default.Register<NavigateToMessage>(this, t => NavigateTo(t));

            #region Server
            if (Data.ThisMacineType == Machine.Server)
			{
                Messenger.Default.Send(new NavigateToMessage(@"ServerView/MainServerPage.xaml"));
                Messenger.Default.Register<LoggingMessage>(this, t => AddLog(t));
                Messenger.Default.Send(new LoggingMessage(KDLogger.Info("Start")));
            }
			#endregion

			#region Client
            if (Data.ThisMacineType != Machine.Server)
			{
                Messenger.Default.Send(new NavigateToMessage(@"RolePage.xaml"));
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

		private void AddLog (LoggingMessage m)
		{
            LogsView.Add(m.Message);
		}

        private void NavigateTo(NavigateToMessage m)
		{
            FrameSource = m.Target;
		}
    }
}