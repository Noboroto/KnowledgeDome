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
            ObstacleList obstacleList= new ObstacleList
            {
                new Obstacle
                {
                    obstacleQuestion = new ObstacleQuestion (1,1,"wethd")
                }
            };
            obstacleList[0].RowList.Add(new ObstacleRowQuestion(2, 1, 2, "abc", "abc"));
            using (StreamWriter sw = File.CreateText(@"C:\Users\thanh\OneDrive\Desktop\abc.json"))
            {
                string text = "[";
                foreach (var child in obstacleList)
                {
                    text += "{" + child.CovertToJson() + "},";
                }
                text = text.Remove(text.Length - 1 ,1);
                text += "]";
                sw.WriteLine(text);
                obstacleList = JsonConvert.DeserializeObject<ObstacleList>(text);
            }
        }
    }
}
