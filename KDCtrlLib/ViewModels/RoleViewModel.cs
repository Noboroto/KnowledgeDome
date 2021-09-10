using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDLib.MessageForUI;

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KDCtrlLib.ViewModels
{
	public class RoleViewModel : ViewModelBase
	{
		#region Private
		private int _Choice = -1;
		private bool _AskRoleEnable;
		private CancellationTokenSource cancellation = new CancellationTokenSource();
		#endregion

		#region Public
		public int Choice
		{
			get => _Choice;
			set
			{
				Set(ref _Choice, value);
				AskRoleEnable = true;
			}
		}
		public bool AskRoleEnable
		{
			get => _AskRoleEnable;
			set => Set(ref _AskRoleEnable, value);
		}

		public ObservableCollection<string> Roles { get; set; }
		#endregion

		#region ICommand
		public RelayCommand AskRole { get; set; }
		#endregion

		public RoleViewModel()
		{
			Roles = new ObservableCollection<string>();
			CommandChecker(cancellation.Token);
			AskRoleEnable = true;
			AskRole = new RelayCommand(
				() =>
				{
					if (Choice < 0) return;
					Data.ID = Choice;
					NetClient.SendCommand(new KDCommand(CommandType.AskForConnect, Data.ID.ToString()));
					KDLogger.Error("Starting");
					AskRoleEnable = false;
				});
		}

		private void CommandChecker(CancellationToken token)
		{
			Task.Run(() =>
			{
				while (true)
				{
					if (token.IsCancellationRequested)
						return;
					if (Data.RoundCommnads.Count > 0)
					{

						KDCommand command = Data.RoundCommnads.Peek();
						switch (command.PrefixCmd)
						{
							case CommandType.ClientList:
								Data.CurrentMatchIndex = int.Parse(command.Content);
								Application.Current.Dispatcher.Invoke(() =>
								{
									foreach (var c in Data.CurrentMatch.Players)
									{
										Roles.Add(c.Name);
									}
									Roles.Add("MC");
									Roles.Add("Khán giả");
								});
								goto EndCommand;
							case CommandType.ConfirmIP:
								Data.ChooseIP = command.OwnIP.ToString();
								Data.Pos = command.Pos;
								goto EndCommand;
							case CommandType.AccpetConnect:
								cancellation.Cancel();
								Messenger.Default.Send(new NavigateToMessage("MainClientPage.xaml"));
								goto EndCommand;
							case CommandType.RefuseConnect:
								MessageBox.Show("Bị từ chối kết nối do đã có người ở vị trí này");
								AskRoleEnable = true;
								goto EndCommand;
							EndCommand:
								if (Data.RoundCommnads.Count > 0) Data.RoundCommnads.Dequeue();
								continue;
							default:
								continue;
						}
					}
				}
			}, token);
		}
	}
}
