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
            AccelerationQuestionList abc = new AccelerationQuestionList();
            abc.Add(new AccelerationQuestion(1, "abcaaD", "ăe"));
            abc.Add(new AccelerationQuestion(2, "acaaD", "ăe"));
            abc.Add(new AccelerationQuestion(3, "aaaD", "dasdăe"));

            using (StreamWriter sw = File.CreateText(@"C:\Users\thanh\OneDrive\Desktop\abc.json"))
            {
                string text = JsonConvert.SerializeObject(abc);
                sw.WriteLine(text);
                abc = JsonConvert.DeserializeObject<AccelerationQuestionList>(text);
            }
        }
    }
}
