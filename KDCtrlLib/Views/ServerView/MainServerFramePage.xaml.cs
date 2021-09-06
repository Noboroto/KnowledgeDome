using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using KDLib;

namespace KDCtrlLib.Views.ServerView
{
    /// <summary>
    /// Interaction logic for MainServerFramePage.xaml
    /// </summary>
    public partial class MainServerFramePage : Page
    {

        public MainServerFramePage()
        {
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
