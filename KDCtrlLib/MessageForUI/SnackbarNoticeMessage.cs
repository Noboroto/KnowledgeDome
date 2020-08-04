using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDCtrlLib.MessageForUI
{
    public class NoticeMessage
    {
        public string Message { get; set; }
        public NoticeMessage(string m)
        {
            Message = m;
        }
    }
}
