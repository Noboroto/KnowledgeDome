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
            Console.WriteLine("Start");
            try
            {
                NetClient.Connect("192.168.12.1");
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine(e.Message);
                }
            }
            TcpClient tb = new TcpClient();
            Console.WriteLine("End " + (tb.Client == null).ToString());
            using (tb = new TcpClient())
            {
                Console.WriteLine("End " + (tb.Client == null).ToString());
            }
            Console.WriteLine("End " + (tb.Client == null).ToString());
            Console.ReadKey();
        }
    }
}
