using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using KDLib;
using KDLib.MessageForUI;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace KDCtrlLib.ViewModels
{
	public class RoleViewModel : ViewModelBase
	{
		#region Private
		private int _Choice = -1;
		private bool _AskRoleEnable;
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

			Data.RoundCommnads = new KDCommandList(CommandChecker);

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

		private void CommandChecker(KDCommand command)
		{
			Task.Run(() =>
			{
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
						break;
					case CommandType.ConfirmIP:
						Data.ChooseIP = command.OwnIP.ToString();
						Data.Pos = command.Pos;
						break;
					case CommandType.AccpetConnect:
						Data.UpdateFromPackage(command.Content);
						Messenger.Default.Send(new NavigateToMessage());
						break;
					case CommandType.RefuseConnect:
						MessageBox.Show(command.Content);
						AskRoleEnable = true;
						break;
					default:
						break;
				}
			});
		}
	}
}
