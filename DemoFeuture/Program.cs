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
            FinishQuestionList abc = new FinishQuestionList()
            {
                new FinishQuestion(1,10, "abcaaD", "ăe"),
                new FinishQuestion(2,20, "acaaD", "ăe"),
                new FinishQuestion(3,20 ,"aaaD", "dasdăe")
            };
            using (StreamWriter sw = File.CreateText(@"C:\Users\thanh\OneDrive\Desktop\abc.json"))
            {
                string text = JsonConvert.SerializeObject(abc);
                sw.WriteLine(text);
                abc = JsonConvert.DeserializeObject<FinishQuestionList>(text);
            }
        }
    }
}
