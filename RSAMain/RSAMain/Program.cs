using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RSALIB;

namespace RSAMain
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RSA rsa = new RSA();
            HashFunction hs = new HashFunction();

            ulong[] k = new ulong[4];
            for (int i = 0; i < 4; i++) k[i] = PrimeNumberGenerator.Generate();

            byte[] array = hs.CreateHashCode("test.docx", "out.txt", k);

            






            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
