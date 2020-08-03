using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using KDLib;
using System.IO;
using System.Net;
using System.Windows;
using KDLib.KDException;
using System.Threading;
using System.Net.Sockets;

namespace DemoFeuture
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            {
                try
                {
                    NetServer.Start();
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
