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
            MainFrame.Navigate(new MainServerPage());
            Dispatcher.Invoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    SendMessage.Focus();
                    Keyboard.Focus(SendMessage);
                }));
            Logging.Items.Add(new LogViewerInfo(MachineType.Server, "test test test"));
            Logging.Items.Add(new LogViewerInfo(MachineType.Server, "test test test"));
            Logging.Items.Add(new LogViewerInfo(MachineType.Server, "test test test"));
            Logging.Items.Add(new LogViewerInfo(MachineType.Server, "test test test"));
            Logging.Items.Add(new LogViewerInfo(MachineType.Server, "test test test"));
            Logging.Items.Add(new LogViewerInfo(MachineType.Server, "test test test"));
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (string.IsNullOrEmpty(SendMessage.Text)) return;
                var item = new LogViewerInfo(MachineType.MC, SendMessage.Text);
                if ((Chatting.Items.Count + 1) % 5 == 0)
                    item = new LogViewerInfo(MachineType.Player, SendMessage.Text);
                else if ((Chatting.Items.Count + 1) % 2 != 0)
                    item = new LogViewerInfo(MachineType.Server, SendMessage.Text);
                Chatting.Items.Add(item);
                Chatting.ScrollIntoView(item);
                SendMessage.Text = "";
            }
        }
    }
}
