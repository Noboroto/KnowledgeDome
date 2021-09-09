using KDLib;

namespace KDLib.MessageForUI
{
    public class LogMess
    {
        public LogViewerInfo Message { get; set; }
        public LogMess(LogViewerInfo m)
        {
            Message = m;
        }
    }
}
