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
            MatchList abc = new MatchList();
            string s = File.ReadAllText(@"C:\Users\thanh\OneDrive\Desktop\abc.json");
            abc = JsonConvert.DeserializeObject<MatchList>(s);
        }
    }
}
