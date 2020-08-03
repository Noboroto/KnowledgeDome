using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDCtrlLib.Interface
{
    public interface IHandleEvent
    {
        Task<string> GetsyncException(Task t);
    }
}
