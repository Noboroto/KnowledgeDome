using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using KDLib;
using System.IO;

namespace DemoFeuture
{
    class Program
    {
        static void Main(string[] args)
        {
            KDCommand a = new KDCommand(MachineType.MC, CommandType.Hide, "aba");
            using (StreamWriter se = File.CreateText(@"C:\Users\thanh\OneDrive\Desktop\bc.json"))
            {
                string t = JsonConvert.SerializeObject(a);
                se.Write(t);
                a = JsonConvert.DeserializeObject<KDCommand>(t);
            }
        }
    }
}
