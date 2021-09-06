using System;
namespace KDCtrlLib.MessageForUI
{
    public class NavigateToMessage
    {
        public Uri Target { get; set; }
        public NavigateToMessage(string target)
        {
            Target = new Uri(@"pack://application:,,,/KDCtrlLib;component/Views/" + target);
        }
    }
}
