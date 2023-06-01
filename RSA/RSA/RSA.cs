using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RSALIB
{

    public class RSA
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct RSApkde
        {
            [FieldOffset(0)]
            public ulong RSAInt;

            [FieldOffset(0)]
            public byte RSAByte1;
            [FieldOffset(1)]
            public byte RSAByte2;
            [FieldOffset(2)]
            public byte RSAByte3;
            [FieldOffset(3)]
            public byte RSAByte4;
        }


        #region keys generation

        //public BigInteger PublicKey_PrivateKey(ulong p, ulong q, out BigInteger d_, out BigInteger e_)
        //{
        //    BigInteger n_;
        //    BigInteger feul;
        //    e_ = 0;
        //    n_ = p * q;
        //    feul = (p - 1) * (q - 1);

        //    d_ = feul - 1;
        //    for (BigInteger i = 2; i <= feul; i++)
        //    {
        //        if ((feul % i == 0) && (d_ % i == 0))
        //        {
        //            d_--;
        //            i = 1;
        //        }
        //    }


        //    do
        //    {
        //        e_ = GenerateRandomCoprime(feul);
        //    } while (e_ == d_);

        //    return n_;
        //}

        //private BigInteger GenerateRandomCoprime(BigInteger feul)
        //{
        //    Random random = new Random();
        //    BigInteger number;
        //    do
        //    {
        //        number = random.Next(2, (int)feul);
        //    } while (BigInteger.GreatestCommonDivisor(number, feul) != 1);

        //    return number;
        //}

        #endregion

        #region new keys generation

        private static BigInteger CalculatePrivateKey(BigInteger openKey, BigInteger feul)
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
            Random random = new Random();
            List<BigInteger> CommonKeys = new List<BigInteger> { 65537, 17 };
            BigInteger openKey = CommonKeys[random.Next(0, CommonKeys.Count)]; // Commonly chosen as the open exponent

            return openKey;
        }

        public BigInteger PublicKey_PrivateKey(ulong p, ulong q, out BigInteger publicKey, out BigInteger privateKey)
        {
            BigInteger n = p * q; // Calculate the product of p and q
            BigInteger feul = (p - 1) * (q - 1); // Calculate Euler's totient function

            publicKey = CalculatePublicKey(feul);
            privateKey = CalculatePrivateKey(publicKey, feul);

            return n;
        }


        #endregion

        private ulong powRSA(ulong byt, BigInteger d_, BigInteger n_)
        {
            BigInteger st = 1;

            for (BigInteger i = 0; i < d_; i++)
            {
                st *= byt;
            }
            st %= n_;
            return (ulong)st;
        }

        public void EncryptFileRSA(string InFile, string OutFile, BigInteger d_, BigInteger n_)
        {
            byte[] array;
            byte[] result;
            RSApkde pkde;
            pkde.RSAByte1 = 0;
            pkde.RSAByte2 = 0;
            pkde.RSAByte3 = 0;
            pkde.RSAByte4 = 0;

            using (FileStream fstream = File.OpenRead(@InFile))
            {
                array = new byte[fstream.Length];
                result = new byte[fstream.Length * 4];
                fstream.Read(array, 0, array.Length);
            }

            for (int i = 0; i < array.Length; i++)
            {
                pkde.RSAInt = powRSA(array[i], d_, n_);
                result[i * 4] = pkde.RSAByte1;
                result[i * 4 + 1] = pkde.RSAByte2;
                result[i * 4 + 2] = pkde.RSAByte3;
                result[i * 4 + 3] = pkde.RSAByte4;
            }

            using (FileStream fstream = new FileStream(@OutFile, FileMode.Create))
            {
                fstream.Write(result, 0, result.Length);
            }
        }

        public void DecipherFileRsa(string InFile, string OutFile, BigInteger e_, BigInteger n_)
        {
            byte[] array;
            byte[] result;
            RSApkde pkde;
            pkde.RSAInt = 0;
            pkde.RSAByte1 = 0;
            pkde.RSAByte2 = 0;
            pkde.RSAByte3 = 0;
            pkde.RSAByte4 = 0;

            using (FileStream fstream = File.OpenRead(@InFile))
            {
                array = new byte[fstream.Length];
                result = new byte[fstream.Length / 4];
                fstream.Read(array, 0, array.Length);
            }

            for (int i = 0; i < result.Length; i++)
            {
                pkde.RSAByte1 = array[i * 4 + 0];
                pkde.RSAByte2 = array[i * 4 + 1];
                pkde.RSAByte3 = array[i * 4 + 2];
                pkde.RSAByte4 = array[i * 4 + 3];
                pkde.RSAInt = powRSA(pkde.RSAInt, e_, n_);
                result[i] = pkde.RSAByte1;
            }

            using (FileStream fstream = new FileStream(OutFile, FileMode.Create))
            {
                fstream.Write(result, 0, result.Length);
            }
        }

    }

}
