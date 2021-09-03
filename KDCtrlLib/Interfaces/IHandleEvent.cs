using System;
using System.Threading.Tasks;

namespace KDCtrlLib.Interface
{
    public interface IHandleExeception
    {
        Task<string> GetException(Task t);
        Task<string> GetException(Action a);
    }
}
