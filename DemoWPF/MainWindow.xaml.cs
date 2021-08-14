using KDCtrlLib.Views;

using KDLib;

using System.Windows;
using System.Windows.Input;

namespace DemoWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ICommand TryConnectCmd { get; set; }
		public MainWindow()
		{
			Data.ClientInitialize();
			InitializeComponent();
			MyFrame.Navigate(new MainClientPage());
		}
	}
}
