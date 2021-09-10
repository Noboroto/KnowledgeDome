using KDLib;

using System.Windows;

namespace KDClient
{
	/// <summary>
	/// Interaction logic for ClientWindow.xaml
	/// </summary>
	public partial class ClientWindow : Window
	{
		public ClientWindow()
		{
			Data.ClientInitialize();
			InitializeComponent();
		}
	}
}
