using KDLib;

using System;
using System.Windows;

namespace DemoFeuture
{
	class Program
	{
		static void Main(string[] args)
		{
			Data.ServerInitialize();
			{
				try
				{
					NetServer.Start().Wait();
				}
				catch (AggregateException ae)
				{
					string s = "";
					foreach (var e in ae.InnerExceptions) s += e.Message + "\n";
					MessageBox.Show(s);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
				}
			}
			Console.ReadKey();
		}
	}
}
