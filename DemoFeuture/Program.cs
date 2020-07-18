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

namespace DemoFeuture
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            try
            {
                NetClient.Connect("192.168.51.17").Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            Console.WriteLine("End");
            Console.ReadKey();
        }
    }
}
