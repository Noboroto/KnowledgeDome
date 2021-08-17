using KDLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KDCtrlLib.KDControl
{
	/// <summary>
	/// Interaction logic for PlayerMainClientView.xaml
	/// </summary>
	public partial class PlayerMainClientView : UserControl
	{
		public static readonly DependencyProperty SizeProperty =
			DependencyProperty.Register("Size", typeof(int),
										typeof(PlayerMainClientView)
										);
		public int Size
		{
			get => (int)GetValue(SizeProperty); 
			set => SetValue(SizeProperty, value); 
		}
		private Player _PlayerData;

		public Player PlayerData
		{
			get => _PlayerData;
			set
			{
				_PlayerData = value;
				DataContext = _PlayerData;
			}
		}
		public ImageSource Source => _PlayerData.Avatar;

		public int RectangleSize => Size + 10;
		public PlayerMainClientView()
		{
			InitializeComponent();
		}
	}
}
