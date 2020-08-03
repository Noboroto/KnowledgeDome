using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDCtrlLib.MessageForUI
{
    public class SnackbarNoticeMessage
    {
        public string Message { get; set; }
        public SnackbarNoticeMessage(string m)
        {
            Message = m;
        }
    }
}
