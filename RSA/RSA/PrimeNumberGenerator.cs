using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSALIB
{
    public static class PrimeNumberGenerator
    {
        private static readonly Random _random = new Random();

        public static ulong Generate()
        {
            var numberBytes = new byte[4];
            _random.NextBytes(numberBytes);
            ulong number = BitConverter.ToUInt32(numberBytes, 0);

            while (!IsPrime(number))
            {
                unchecked
                {
                    number++;
                }
            }
            return number;
        }
        

        private static bool IsPrime(ulong number)
        {
            if ((number & 1) == 0) return (number == 2);

            var limit = (ulong)Math.Sqrt(number);
            for (ulong i = 3; i <= limit; i += 2)
            {
                if ((number % i) == 0) return false;
            }
            return true;
        }


    }

}
