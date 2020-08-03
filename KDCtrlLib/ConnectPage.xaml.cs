using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using KDCtrlLib.MessageForUI;
using System.Threading.Tasks;
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
            OnNavigatedTo();
        }

        protected void OnNavigatedTo()
        {
            Messenger.Default.Register<SnackbarNoticeMessage>(this, m => ReceiveChangeBackgroundMessage(m));
        }

        protected void OnNavigatingFrom()
        {
            Messenger.Default.Register<SnackbarNoticeMessage>(this, m => ReceiveChangeBackgroundMessage(m));
        }

        public void ReceiveChangeBackgroundMessage(SnackbarNoticeMessage snack)
        {
            var queue = NoticeBar.MessageQueue;
            Task.Factory.StartNew(() => queue.Enqueue(snack.Message));
        }
    }
}
