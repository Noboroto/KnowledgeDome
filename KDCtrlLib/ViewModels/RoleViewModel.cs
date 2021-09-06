using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using KDCtrlLib.MessageForUI;

using KDLib;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KDCtrlLib.ViewModels
{
    public class RoleViewModel : ViewModelBase
    {
        #region ICommand
        public ICommand AskPermision { get; set; }
        #endregion

        private string _Choice = "";
        public string Choice
		{
			get => _Choice;
			set => Set(ref _Choice, value);
		}
		public ObservableCollection<string> Roles { get; set; }


        public RoleViewModel()
        {
            Roles = new ObservableCollection<string>();
            AskPermision = new RelayCommand(
                () =>
                {
                    switch (Choice)
					{
                        case "MC":
                            Data.ThisMacineType = Machine.MC;
                            break;
                        case "Khán giả":
                            Data.ThisMacineType = Machine.Viewer;
                            break;
                        default:
                            Data.ThisMacineType = Machine.Player;
                            break;
					}
                },
                () =>
                {
                    return !string.IsNullOrEmpty(Choice);
                }
           );
        }

        public Task ProcessCommand()
		{
            Action ThisAction = () =>
            {
                while (true)
                {
                    if (Data.Commands.Count > 0)
                    {
                        KDCommand command = Data.Commands.Peek();
                        switch (command.PrefixCmd)
                        {
                            case CommandType.ClientList:
                                foreach (var c in Data.FromJosn<ObservableCollection<string>>(command.Content))
                                {
                                    Roles.Add(c);
                                }
                                goto EndCommand;
                            EndCommand:
                                if (Data.Commands.Count > 0) command = Data.Commands.Dequeue();
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            };
            return Task.Run(ThisAction);
        }
    }
}
