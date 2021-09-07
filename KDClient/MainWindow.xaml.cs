using System.Windows;

using KDLib;

namespace KDClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Data.ClientInitialize();
		}
	}
}
