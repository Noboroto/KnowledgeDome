using KDLib;

using System.Windows;

namespace KDServer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Data.ServerInitialize();
		}
	}
}
