using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using KDCtrlLib.Interface;
using KDCtrlLib.MessageForUI;
using KDLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KDCtrlLib.ViewModel
{
    public class RoleViewModel : ViewModelBase, IHandleExeception
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

        public Task<string> GetException(Task t)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetException(Action a)
        {
            throw new NotImplementedException();
        }
    }
}
