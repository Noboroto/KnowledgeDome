using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDLib.MessageForUI;

using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
	public class ConnectViewModel : ViewModelBase
	{
		private IPAddress _ServerIP;
		private const string AcceptString = "Kết nối thành công";

		public string IP
		{
			get => ConfigurationSettings.IP;
			set
			{
				ConfigurationSettings.IP = value;
				ConfigurationSettings.Save();
				RaisePropertyChanged(nameof(IP));
			}
		}

		#region Command
		public ICommand TryConnectCmd { get; set; }
		public ICommand TryConnectEnterCmd { get; set; }
		#endregion

		public ConnectViewModel()
		{
			TryConnectCmd = new RelayCommand
			(
				() =>
				{
					Connect(IP);
				},
				() =>
				{
					return IsValidIPString(IP);
				}
			);
			TryConnectEnterCmd = new RelayCommand<KeyEventArgs>(e =>
			{
				if (e.Key == Key.Enter)
				{
					if (IsValidIPString(IP))
					{
						Connect(IP);
					}
				}
			});
		}

		public async void Connect(string IP)
		{
			foreach (var localip in NetServer.GetLocalIPAddress())
			{
				if (NetClient.CheckSameNetwork(localip, IP))
				{
					Data.ChooseIP = localip;
					break;
				}
			}
			if (await NetClient.IsValidConnection(_ServerIP))
			{
				Messenger.Default.Send(new NavigateToMessage(@"RolePage.xaml"));
			}
			else MessageBox.Show("Không tìm thấy địa chỉ IP");
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
			return IPAddress.TryParse(s, out _ServerIP);
		}
	}
}
