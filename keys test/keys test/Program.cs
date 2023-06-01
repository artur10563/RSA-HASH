using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace keys_test
{
    internal class Program
    {


        private static BigInteger GeneratePrime()
        {
            Random random = new Random();
            BigInteger number;

            do
            {
                byte[] bytes = new byte[4];
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; // Ensure the highest bit is not set
                number = new BigInteger(bytes);
            } while (!IsPrime(number));

            return number;
        }

        private static bool IsPrime(BigInteger number)
        {
            if (number < 2) return false;

            for (BigInteger i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }




        private static BigInteger CalculateClosedKey(BigInteger openKey, BigInteger feul)
        {
            BigInteger a = openKey;
            BigInteger b = feul;
            BigInteger x = 0;
            BigInteger y = 1;
            BigInteger lastX = 1;
            BigInteger lastY = 0;

            while (b != BigInteger.Zero)
            {
                BigInteger quotient = a / b;

                BigInteger temp = a;
                a = b;
                b = temp % b;

                temp = x;
                x = lastX - quotient * x;
                lastX = temp;

                temp = y;
                y = lastY - quotient * y;
                lastY = temp;
            }

            if (lastX < BigInteger.Zero)
            {
                lastX += feul;
            }

            return lastX;
        }

        private static BigInteger CalculatePublicKey(BigInteger feul)
        {
            BigInteger openKey = 65537; // Commonly chosen as the open exponent

            return openKey;
        }

        static void Main(string[] args)
        {
            BigInteger p = GeneratePrime(); // Generate first prime number
            BigInteger q = GeneratePrime(); // Generate second prime number

            BigInteger n = p * q; // Calculate the product of p and q
            BigInteger feul = (p - 1) * (q - 1); // Calculate Euler's totient function

            BigInteger openKey = CalculatePublicKey(feul); // Select the open exponent
            BigInteger closedKey = CalculateClosedKey(openKey, feul); // Calculate the closed exponent

            Console.WriteLine($"p: {p}");
            Console.WriteLine($"q: {q}");
            Console.WriteLine($"n: {n}");
            Console.WriteLine($"feul: {feul}");
            Console.WriteLine($"openKey: {openKey}");
            Console.WriteLine($"closedKey: {closedKey}");

            Console.ReadLine();
        }

    }
}

