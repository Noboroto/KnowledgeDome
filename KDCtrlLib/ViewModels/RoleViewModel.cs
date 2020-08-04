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
        public ICommand AskPermision { get; set; }
        #endregion

        public InfoToChoose Choise { get; set; }

        public RoleViewModel()
        {
            AskPermision = new RelayCommand<int>(
                (i) =>
                {
                    if (i == -1) Messenger.Default.Send(new NoticeMessage("Chưa chọn chức năng client!"));
                });
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
