using GalaSoft.MvvmLight;

using System;
using System.Windows.Media;

namespace KDLib
{
	public class LogViewerInfo : ObservableObject
	{
		private string _Message;
		private MachineType _machineType;
		private DateTime _dateTime;

		public Brush ForegroundColor
		{
			get
			{
				switch (_machineType)
				{
					case MachineType.MC:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03a9f4"));
					case MachineType.Server:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8bc34a"));
					default:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF0808"));
				}
			}
		}
		public string Info => _dateTime.ToString("HH:mm:ss - ") + _machineType.ToString() + ": ";
		public string Message => _Message;
		public LogViewerInfo(MachineType machine, string mess)
		{
			Set(ref _dateTime, DateTime.Now);
			Set(ref _machineType, machine);
			Set(ref _Message, mess);
		}
	}
}
