using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RSALIB;

//DES(M || RSA_A(H(M))) || RSA_B(KS)
namespace RSAMain
{
    internal class Program
    {


        private static void ShowDict<K, V>(Dictionary<K, V> d)
        {
            foreach (var item in d)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
        }


        private static RSA rsa = new RSA();
        private static SymetricEncryptionByte des = new SymetricEncryptionByte();
        private static HashFunction hf = new HashFunction();

        static void Main(string[] args)
        {

            //Create test text file 
            string plainTextPath = "plainText.txt";
            string message = "test message";
            File.WriteAllText(plainTextPath, message);

            //Console.WriteLine(Encoding.Default.GetString(M)); // з байтів в строку 

            //H(M)
            #region H(M)

            //4 ulongs for HashFunction
            ulong[] iv = new ulong[4]; for (int i = 0; i < iv.Length; i++) iv[i] = PrimeNumberGenerator.Generate();
            string hashTextPath = "hasText.txt";

            byte[] hashM = hf.CreateHashCode(plainTextPath, hashTextPath, iv);

            #endregion

            //A Keys
            #region A Keys

            BigInteger PublicKeyA;
            BigInteger PrivateKeyA;
            BigInteger n_a = rsa.PublicKey_PrivateKey(43, 59, out PublicKeyA, out PrivateKeyA); //43, 59 => PrimeNumberGenerator.Generate();

            #endregion

            //RSA_A(H(M))
            #region RSA_A(H(M))

            string rsaAHashPath = "rsaAHash.txt";
            rsa.EncryptFileRSA(hashTextPath, rsaAHashPath, PublicKeyA, n_a);

            #endregion

            //M || RSA_A(H(M))
            #region M || RSA_A(H(M))

            byte[] M = File.ReadAllBytes(plainTextPath);
            byte[] rsaHash = File.ReadAllBytes(rsaAHashPath);
            byte[] rsaHashPlusMessage = rsaHash.Concat(M).ToArray();

            string rsaHashPlusMessagePath = "rsaHashPlusMessage.txt";
            File.WriteAllBytes(rsaHashPlusMessagePath, rsaHashPlusMessage);

            #endregion

            //DES(M || RSA_A(H(M))), Session key
            #region DES(M || RSA_A(H(M))), Skey

            string desPath = "des.txt";
            int addByte = 0;

            ulong k = PrimeNumberGenerator.Generate();
            byte[] sesKey = new byte[8];
            byte[] array = des.UlongToByte((long)k);
            Array.Copy(array, 0, sesKey, 0, 4);
            k = PrimeNumberGenerator.Generate();
            array = des.UlongToByte((long)k);
            Array.Copy(array, 0, sesKey, 4, 4);

            byte[] decodeKey = des.EncryptFile(rsaHashPlusMessagePath, desPath, sesKey, out addByte);
            string sesKeyPath = "sesKey.txt";
            File.WriteAllBytes(sesKeyPath, sesKey);

            #endregion

            //B Keys
            #region B Keys

            BigInteger PublicKeyB;
            BigInteger PrivateKeyB;
            BigInteger n_b = rsa.PublicKey_PrivateKey(43, 59, out PublicKeyB, out PrivateKeyB); //43, 59 => PrimeNumberGenerator.Generate();

            #endregion

            //RSA_B(sesKey)
            #region RSA_B(sesKey)

            string rsaSesKeyPath = "rsaBSes.txt";
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! n_a or n_b, PublicB or PrivateB
            rsa.EncryptFileRSA(sesKeyPath, rsaSesKeyPath, PublicKeyB, n_b);

           

            #endregion

            //DES(M || RSA_A(H(M))) || RSA_B(sesKey)
            #region DES(M || RSA_A(H(M))) || RSA_B(sesKey)

            string finalPath = "final.txt";

            byte[] d = File.ReadAllBytes(desPath);
            byte[] rb = File.ReadAllBytes(rsaSesKeyPath);
            byte[] final = d.Concat(rb).ToArray();
            File.WriteAllBytes(finalPath, final);

            //Console.WriteLine($"d: {d.Length}\nrb: {rb.Length}\nfinal: {final.Length}");

            #endregion


            //не працює, але похоже
            //(сокети)передаємо  зашифроване повідомлення на сервер Б, передаємо довжину RSA_B(sesKey)

            string sesKeyBPath = "sesKeyB.txt";

            int rsaSesKeyLength = rb.Length; //нова змінна, бо ми передаємо тільки довжину, а не сам ключ 
            byte[] sesKeyB = new byte[rsaSesKeyLength];

            Array.Copy(final, final.Length - rsaSesKeyLength, sesKeyB, 0, rsaSesKeyLength);

            File.WriteAllBytes(sesKeyBPath, sesKeyB);

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}