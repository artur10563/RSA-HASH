using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RSALIB;

namespace RSAMain
{
    internal class Program
    {
        

        private static void ShowDict<K, V>(Dictionary<K,V> d) 
        {
            foreach (var item in d)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
        }
        static void Main(string[] args)
        {

            var p = WMI.GetProcessorInfo();
            var b = WMI.GetBaseBoardInfo();
            

            Console.WriteLine(p.GetHashCode());
            Console.WriteLine(b.GetHashCode());





            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
