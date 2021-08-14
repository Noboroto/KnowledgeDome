using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using KDCtrlLib.MessageForUI;

namespace KDCtrlLib.ViewModels
{
	public class RoleViewModel : ViewModelBase
	{
		#region ICommand
		public RelayCommand<string> AskPermision { get; set; }
		#endregion

		private string _Choice = "";
		public string Choice
		{
			get
			{
				return _Choice;
			}
			set
			{
				Set(nameof(Choice), ref _Choice, value);
				if (!string.IsNullOrEmpty(_Choice)) AskPermision.RaiseCanExecuteChanged();
			}
		}

		public RoleViewModel()
		{
			AskPermision = new RelayCommand<string>(
				(s) =>
				{
					Messenger.Default.Send(new NoticeMessage(s));
				},
				(s) =>
				{
					return !string.IsNullOrEmpty(s);
				}
		   );
		}
	}
}
