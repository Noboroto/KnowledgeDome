using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using KDLib.MessageForUI;

using KDLib;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Threading;

namespace KDCtrlLib.ViewModels
{
	public class RoleViewModel : ViewModelBase
	{
		#region Privat
		private int _Choice = -1;
		private CancellationTokenSource cancellation;
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
			cancellation = new CancellationTokenSource();
			ProcessCommand(cancellation.Token);
			AskRoleEnable = false;
			AskRole = new RelayCommand(
				() =>
				{
					Data.ID = Choice;
					NetClient.SendCommand(new KDCommand(CommandType.AskForConnect, Data.ID.ToString()));
					AskRoleEnable = false;
				});
		}

		private void ProcessCommand(CancellationToken token)
		{
			Task.Run(() =>
			{
				while (true)
				{
					if (token.IsCancellationRequested) return;
					if (Data.Commands.Count > 0)
					{
						try
						{
							KDCommand command = Data.Commands.Peek();
							switch (command.PrefixCmd)
							{
								case CommandType.ClientList:
									foreach (var c in Data.FromJosn<ObservableCollection<string>>(command.Content))
									{
										Application.Current.Dispatcher.Invoke(() => Roles.Add(c));
									}
									goto EndCommand;
								case CommandType.AccpetConnect:
									cancellation.Cancel();
									Messenger.Default.Send(new NavigateToMessage("MainClientPage.xaml"));
									goto EndCommand;
								case CommandType.RefuseConnect:
									MessageBox.Show("Bị từ chối kết nối do đã có người ở vị trí này");
									goto EndCommand;
								EndCommand:
									if (Data.Commands.Count > 0) Data.Commands.Dequeue();
									continue;
								default:
									continue;
							}
						}
						catch (NullReferenceException)
						{
							continue;
						}
						catch (InvalidOperationException)
						{
							continue;
						}
					}
				}
			}, token);
		}
	}
}
