using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using KDCtrlLib.Interface;
using KDCtrlLib.MessageForUI;

using KDLib;
using KDLib.KDException;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
    public class ConnectViewModel : ViewModelBase
    {
        #region Command
        private IPAddress ServerIP;
        private const string AcceptString = "Kết nối thành công";
        public ICommand TryConnectCmd { get; set; }
        #endregion

        public ConnectViewModel()
        {
            TryConnectCmd = new RelayCommand<string>
            (
                async (s) =>
                {
                    if (await NetClient.IsValidConnection(ServerIP))
                    {
                        Messenger.Default.Send(new NavigateToMessage(@"ServerView/MainServerPage.xaml"));
                        ConfigurationSettings.IP = s;
                        ConfigurationSettings.Save();
                        await NetClient.Connect(ServerIP);
                    }
                    else MessageBox.Show("Không tìm thấy địa chỉ IP");
                },
                (s) =>
                {
                    return IsValidIPString(s);
                }
            );
        }

        private bool IsValidIPString(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s.Count(c => c == '.') < 3) return false;
            var SlitArray = s.Split('.');
            foreach (var part in SlitArray)
            {
                if (part.Length < 1) return false;
            }
            return IPAddress.TryParse(s, out ServerIP);
        }
    }
}
