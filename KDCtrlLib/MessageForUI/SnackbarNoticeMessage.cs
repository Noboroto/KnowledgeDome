using KDLib;

namespace KDCtrlLib.MessageForUI
{
    public class LoggingMessage
    {
        public LogViewerInfo Message { get; set; }
        public LoggingMessage(LogViewerInfo m)
        {
            Message = m;
        }
    }
}
