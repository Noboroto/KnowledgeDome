using System;
using System.IO;
using System.Text;

using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Config;
using log4net.Repository.Hierarchy;
using log4net.Filter;

using System.Linq;
using System.Text.RegularExpressions;

namespace KDLib
{
	public class KDLogger
	{
		private static ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static void Initialize()
		{
			if (!Directory.Exists(@"logs\")) Directory.CreateDirectory(@"logs\");
			PatternLayout patternLayout = new PatternLayout();
			patternLayout.ConversionPattern = "%date{HH:mm:ss,fff} [%thread] %level - %message%newline";
			patternLayout.ActivateOptions();

			var filter = new LevelRangeFilter
			{
				LevelMin = log4net.Core.Level.Warn,
				LevelMax = log4net.Core.Level.Fatal,
			};
			filter.ActivateOptions();

			var appender = new FileAppender();
			appender.AppendToFile = true;

			string name = string.Format("{0:yyyyMMdd}", DateTime.Now);
			var count = Directory.GetFiles(@"logs\").Count(path => Regex.IsMatch(path, $"{name}.*"));
			if (Data.ThisMacineType != Machine.Server) appender.AddFilter(filter);
			appender.File = @"logs\" + $"{name}-{count}.log";
			appender.Encoding = Encoding.UTF8;
			appender.Layout = patternLayout;
			appender.ActivateOptions();

			BasicConfigurator.Configure(appender);
		}
		public static LogViewerInfo MCChat(string message, string ip = "")
		{
			log.Info($"{LogType.MC} {message}");
			return new LogViewerInfo(LogType.MC, message, ip);
		}
		public static LogViewerInfo ServerChat (string message)
		{
			log.Info($"{LogType.Server} {message}");
			return new LogViewerInfo(LogType.Server, message);
		}
		public static LogViewerInfo Info(string message, LogType type = LogType.Server, string note = "")
		{
			log.Info(message);
			return new LogViewerInfo(type, message, note);
		}
		public static LogViewerInfo Error(string message)
		{
			log.Error(message);
			return new LogViewerInfo(LogType.Error, message);
		}
		public static LogViewerInfo Warn(string message)
		{
			log.Error(message);
			return new LogViewerInfo(LogType.Warn, message);
		}
	}
}
