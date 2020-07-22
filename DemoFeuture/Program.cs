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
            var abc = new StartQuestion("Sinh học", "a", "cad", 12);
            string a = JsonConvert.SerializeObject(abc);
            using (StreamWriter stream = new StreamWriter(File.Open(@"ABC.json", FileMode.Create)))
            {
                stream.WriteLine(a);
                var x = JsonConvert.DeserializeObject<StartQuestion>(a);
            }
        }
    }
}
