using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KDCtrlLib;
using KDLib;

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
            Data.Initialize();
            InitializeComponent();
            MyFrame.Navigate(new ConnectPage());
        }
    }
}
