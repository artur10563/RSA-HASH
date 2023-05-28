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

        //    Random random = new Random();
        //    do
        //    {
        //        e_ = GenerateRandomCoprime(feul, random);
        //    } while (e_ == d_);

        //    return n_;
        //}

        //private BigInteger GenerateRandomCoprime(BigInteger feul, Random random)
        //{
        //    BigInteger number;
        //    do
        //    {
        //        number = new BigInteger(random.Next(2, (int)feul));
        //    } while (BigInteger.GreatestCommonDivisor(number, feul) != 1);

        //    return number;
        //}


        public BigInteger PublicKey_PrivateKey(ulong p, ulong q, out BigInteger d_, out BigInteger e_)
        {
            BigInteger n_;
            BigInteger feul;
            e_ = 0;
            n_ = p * q;
            feul = (p - 1) * (q - 1);

            d_ = feul - 1;
            for (BigInteger i = 2; i <= feul; i++)
                if ((feul % i == 0) && (d_ % i == 0))
                {
                    d_--;
                    i = 1;
                }

            //шоб ключі різні були
            e_ = d_ + n_ / 3 * 2;

            //e_ = 10;
            //while (true)
            //{
            //    if ((e_ * d_) % feul == 1)
            //        break;
            //    else
            //        e_++;
            //}

            return n_;

        }


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
