using GalaSoft.MvvmLight.Messaging;

using KDCtrlLib.MessageForUI;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace KDCtrlLib.Views
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
			if (!string.IsNullOrEmpty(snack.Message)) Task.Run(() => queue.Enqueue(snack.Message));
		}

		public void NavigateTo(NavigateToMessage t)
		{
			NavigationService.Navigate(t.Target);
		}

		private void IPText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Enter:
					MessageBox.Show(e.Key.ToString());
					break;
			}
		}
	}
}
