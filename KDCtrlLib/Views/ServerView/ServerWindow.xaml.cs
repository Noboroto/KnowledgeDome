using KDLib;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace KDClient
{
	/// <summary>
	/// Interaction logic for MainServerControlWindow.xaml
	/// </summary>
	public partial class MainServerControlWindow : Window
	{
		public MainServerControlWindow()
		{
			Data.ServerInitialize();
			InitializeComponent();
			Dispatcher.Invoke(DispatcherPriority.Input,
				new Action(delegate ()
				{
					SendMessage.Focus();
					Keyboard.Focus(SendMessage);
				}));
		}
	}
}
