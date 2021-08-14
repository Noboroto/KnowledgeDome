using GalaSoft.MvvmLight.Messaging;

using KDCtrlLib.MessageForUI;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KDCtrlLib.Views
{
	/// <summary>
	/// Interaction logic for RolePage.xaml
	/// </summary>
	public partial class RolePage : Page
	{
		public RolePage()
		{
			InitializeComponent();
		}

		protected void OnNavigatedTo(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Register<NoticeMessage>(this, m => ReceiveChangeBackgroundMessage(m));
			Messenger.Default.Register<NavigateToMessage>(this, t => NavigateTo(t));
		}

		protected void OnNavigatingFrom(object sender, RoutedEventArgs e)
		{
			Messenger.Default.Unregister<NoticeMessage>(this, m => ReceiveChangeBackgroundMessage(m));
			Messenger.Default.Unregister<NavigateToMessage>(this, t => NavigateTo(t));
		}

		public void ReceiveChangeBackgroundMessage(NoticeMessage snack)
		{
			var queue = NoticeBar.MessageQueue;
			if (!string.IsNullOrEmpty(snack.Message)) Task.Run(() => queue.Enqueue(snack.Message));
		}

		public void NavigateTo(NavigateToMessage t)
		{
			NavigationService.Navigate(t.Target);
		}
	}
}
