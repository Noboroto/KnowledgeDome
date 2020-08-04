using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KDCtrlLib.Interface;
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
            AskPermision = new RelayCommand(
                () =>
                {
                    MessageBox.Show(Choise.Name);
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
