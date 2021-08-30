using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using KDCtrlLib.Interface;
using KDCtrlLib.MessageForUI;
using KDCtrlLib.Views;

using KDLib;
using KDLib.KDException;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
    public class ConnectViewModel : ViewModelBase, IHandleExeception
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
                        Messenger.Default.Send(new NavigateToMessage(new RolePage()));
                        ConfigurationSettings.IP = s;
                        ConfigurationSettings.Save();
                        await NetClient.Connect(ServerIP);
                    }
                    else Messenger.Default.Send(new NoticeMessage(IPNotFoundException.message));
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

        public async Task<string> GetException(Task t)
        {
            string s = AcceptString;
            await Task.Run(() =>
            {
                while (!t.IsCompleted) { }
                if (t.Status == TaskStatus.Faulted)
                {
                    s = "";
                    foreach (var e in t.Exception.InnerExceptions)
                    {
                        s += e.Message + "\n";
                    }
                }
            });
            return s;
        }

        public async Task<string> GetException(Action t)
        {
            string s = AcceptString;
            try
            {
                t();
            }
            catch (AggregateException ae)
            {
                s = "";
                foreach (var e in ae.InnerExceptions) s += e.Message + "\n";
            }
            catch (Exception e)
            {
                s = e.Message;
            }
            await Task.Delay(0);
            return s;
        }
    }
}
