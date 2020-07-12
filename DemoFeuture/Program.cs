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
            StartQuestionList abc = new StartQuestionList();
            abc.Add(new StartQuestion(1, "D","abcaaD", "ăe"));
            abc.Add(new StartQuestion(2, "Toán học", "abcaaD", "ăe"));
            abc.Add(new StartQuestion(3, "D", "abcaaD", "ăe"));

            using (StreamWriter sw = File.CreateText(@"C:\Users\thanh\OneDrive\Desktop\abc.json"))
            {
                string text = JsonConvert.SerializeObject(abc);
                sw.WriteLine(text);
                abc = JsonConvert.DeserializeObject<StartQuestionList>(text);
            }
        }
    }
}
