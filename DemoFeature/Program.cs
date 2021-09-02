using KDLib;

using System;
using System.Windows;

namespace DemoFeature
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = Console.ReadLine();
            string client = Console.ReadLine();
            string subnet = NetClient.GetSubnetMask(System.Net.IPAddress.Parse(server)).ToString();
            if (NetClient.CheckWhetherInSameNetwork(server, subnet, client))
            {
                Console.WriteLine("OK");
            }
            else Console.WriteLine("NO");
            Console.ReadKey();
        }
    }
}
