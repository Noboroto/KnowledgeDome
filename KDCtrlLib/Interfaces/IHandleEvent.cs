using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDCtrlLib.Interface
{
    public interface IHandleExeception
    {
        Task<string> GetException(Task t);
        Task<string> GetException(Action a);
    }
}
