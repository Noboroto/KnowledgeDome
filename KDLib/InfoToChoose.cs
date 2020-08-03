using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDLib
{
    public class InfoToChoose : ObservableObject
    {
        #region PrivateMembers
        private string _Name;
        #endregion

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                Set(nameof(Name), ref _Name, value);
            }
        }

        public InfoToChoose (string name)
        {
            Name = name;
        }
    }
}
