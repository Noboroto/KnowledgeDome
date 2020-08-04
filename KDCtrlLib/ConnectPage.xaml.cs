using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using KDCtrlLib.MessageForUI;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KDCtrlLib
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ConnectPage : Page
    {
        public ConnectPage()
        {
            InitializeComponent();
        }

        protected void OnNavigatedTo(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<NoticeMessage>(this, m => ReceiveChangeBackgroundMessage(m));
            Messenger.Default.Register<NavigateToMessage>(this, t => NavigateTo(t));
            IPText.Focus();
        }

        protected void OnNavigatingFrom(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<NoticeMessage>(this, m => ReceiveChangeBackgroundMessage(m));
            Messenger.Default.Unregister<NavigateToMessage>(this, t => NavigateTo(t));
        }

        public void ReceiveChangeBackgroundMessage(NoticeMessage snack)
        {
            var queue = NoticeBar.MessageQueue;
            Task.Factory.StartNew(() => queue.Enqueue(snack.Message));
        }

        public void NavigateTo (NavigateToMessage t)
        {
            this.NavigationService.Navigate(t.Target);
        }
    }
}
