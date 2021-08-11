using System.Windows.Controls;

namespace KDCtrlLib.MessageForUI
{
	public class NavigateToMessage
	{
		public Page Target { get; set; }
		public NavigateToMessage(Page target)
		{
			Target = target;
		}
	}
}
