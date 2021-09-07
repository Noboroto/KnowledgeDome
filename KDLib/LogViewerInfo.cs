using GalaSoft.MvvmLight;

using System;
using System.Windows.Media;

namespace KDLib
{
	public class LogViewerInfo : ObservableObject
	{
		private string _Note;
		private string _Message;
		private LogType _logtype;
		private DateTime _dateTime;

		public Brush ForegroundColor
		{
			get
			{
				switch (_logtype)
				{
					case LogType.Player:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cc5869"));
					case LogType.MC:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03a9f4"));
					case LogType.Server:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8bc34a"));
					case LogType.Viewer:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#69beff"));
					case LogType.Warn:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0dd1b"));
					default:
						return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0000"));
				}
			}
		}
		public string Info => _dateTime.ToString("HH:mm:ss-") + _logtype.ToString() + _Note + ": ";
		public string Message => _Message;
		public LogType Type => _logtype;
		public LogViewerInfo(LogType logtype, string mess, string note = "")
		{
			Set(ref _dateTime, DateTime.Now);
			Set(ref _logtype, logtype);
			Set(ref _Message, mess);
			Set(ref _Note,((note != "") ? "-" : "")+ note);
		}
	}
}
