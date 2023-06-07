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
        private static void CompareFiles(string f1, string f2)
        {
            if (File.Exists(f1) && File.Exists(f2))
            {
                byte[] contents1 = File.ReadAllBytes(f1);
                byte[] contents2 = File.ReadAllBytes(f2);

                if (contents1.SequenceEqual(contents2))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{f1} and {f2} contents are equal");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{f1} and {f2} contents are different");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        private static RSA rsa = new RSA();
        private static SymetricEncryptionByte des = new SymetricEncryptionByte();
        private static HashFunction hf = new HashFunction();


        static void Main(string[] args)
        {





            //Create test text file 
            string plainTextPath = "plainText.txt";
            byte[] message = Encoding.Default.GetBytes("test message");
            File.WriteAllBytes(plainTextPath, message);
            int messageLength = File.ReadAllBytes(plainTextPath).Length;



            //H(M)
            #region H(M)

            //4 ulongs for HashFunction
            ulong[] iv = new ulong[4]; for (int i = 0; i < iv.Length; i++) iv[i] = PrimeNumberGenerator.Generate();
            string hashTextPath = "1hashText.txt";

            byte[] hashM = hf.CreateHashCode(plainTextPath, hashTextPath, iv);

            #endregion

            //A Keys
            #region A Keys

            BigInteger PublicKeyA;
            BigInteger PrivateKeyA;
            //ulong pa = 43;
            //ulong qa = 59;
            ulong pa = PrimeNumberGenerator.Generate();
            ulong qa = PrimeNumberGenerator.Generate();

            BigInteger n_a = rsa.PublicKey_PrivateKey(pa, qa, out PublicKeyA, out PrivateKeyA); //43, 59 => PrimeNumberGenerator.Generate();
            Console.WriteLine("A keys generated");





            #endregion

            //RSA_A(H(M))
            #region RSA_A(H(M))

            string rsaAHashPath = "2rsaAHash.txt";
            rsa.EncryptFileRSA(hashTextPath, rsaAHashPath, PrivateKeyA, n_a);
            Console.WriteLine("rsaAHash.txt is done");
            #endregion

            //M || RSA_A(H(M))
            #region M || RSA_A(H(M))

            byte[] M = File.ReadAllBytes(plainTextPath);
            byte[] rsaHash = File.ReadAllBytes(rsaAHashPath);
            byte[] rsaHashPlusMessage = rsaHash.Concat(M).ToArray();

            string rsaHashPlusMessagePath = "3rsaHashPlusMessage.txt";
            File.WriteAllBytes(rsaHashPlusMessagePath, rsaHashPlusMessage);

            #endregion

            //DES(M || RSA_A(H(M))), Session key
            #region DES(M || RSA_A(H(M))), Skey

            string desPath = "4des.txt";
            int addByte = 0;

            //generating key
            ulong k = PrimeNumberGenerator.Generate();
            byte[] sesKey = new byte[8];
            byte[] array = des.UlongToByte((long)k);
            Array.Copy(array, 0, sesKey, 0, 4);
            k = PrimeNumberGenerator.Generate();
            array = des.UlongToByte((long)k);
            Array.Copy(array, 0, sesKey, 4, 4);

            des.EncryptFile(rsaHashPlusMessagePath, desPath, sesKey, out addByte);

            //save session key
            string sesKeyPath = "5sesKey.txt";
            File.WriteAllBytes(sesKeyPath, sesKey);

            #endregion

            //B Keys
            #region B Keys

            BigInteger PublicKeyB;
            BigInteger PrivateKeyB;
            BigInteger n_b = rsa.PublicKey_PrivateKey(43, 59, out PublicKeyB, out PrivateKeyB); //43, 59 => PrimeNumberGenerator.Generate();
            Console.WriteLine("b keys generated");

            #endregion

            //RSA_B(sesKey)
            #region RSA_B(sesKey)

            string rsaSesKeyPath = "6rsaBSes.txt";

            rsa.EncryptFileRSA(sesKeyPath, rsaSesKeyPath, PublicKeyB, n_b);



            #endregion

            //DES(M || RSA_A(H(M))) || RSA_B(sesKey)
            #region DES(M || RSA_A(H(M))) || RSA_B(sesKey)

            string finalPath = "7final.txt";

            byte[] d = File.ReadAllBytes(desPath);
            byte[] rb = File.ReadAllBytes(rsaSesKeyPath);
            byte[] final = d.Concat(rb).ToArray();
            File.WriteAllBytes(finalPath, final);



            #endregion


            #region server B

            //(сокети)передаємо  зашифроване повідомлення на сервер Б, передаємо довжину RSA_B(sesKey), , M.Len 

            //розшифровуємо ключ 

            string rsaSesKeyBPath = "6.1rsaSesKeyB.txt";

            int rsaSesKeyLength = rb.Length; //нова змінна, бо ми передаємо тільки довжину, а не сам ключ 
            byte[] rsaSesKeyB = new byte[rsaSesKeyLength];

            Array.Copy(final, final.Length - rsaSesKeyLength, rsaSesKeyB, 0, rsaSesKeyLength);

            //файли однакові 6 i 6.1 (рса ключа сесії)
            File.WriteAllBytes(rsaSesKeyBPath, rsaSesKeyB);


            string decipheredSesKeyPath = "5.1decipheredKey.txt";

            //розшифрували  5.1 і 5 (розшифрований ключ сесії)
            rsa.DecipherFileRsa(rsaSesKeyBPath, decipheredSesKeyPath, PrivateKeyB, n_b);

            byte[] decipheredKey = File.ReadAllBytes(decipheredSesKeyPath);

            //розшифрували ключ, віднімаємо від всього переданого зашифрованого повідомлення ключ, отримуємо частину з десом

            byte[] desPart = new byte[final.Length - rsaSesKeyLength];
            Array.Copy(final, 0, desPart, 0, final.Length - rsaSesKeyLength);

            //Деси сходяться, див файли 4.1desPart.txt i 4des.txt
            string desPartPath = "4.1desPart.txt";
            File.WriteAllBytes(desPartPath, desPart);

            //розшифровуємо дес, отримаємо M || RSA_A(H(M))
            //файли rsaHashPlusMessage, decipheredDes
            //decodeKey не можна передавати, треба його витягнути з ключа сесії 5.1

            //3rsaHashPlusMessage i 3.1decipheredDes
            string decipheredDesPath = "3.1decipheredDes.txt";
            decipheredKey = des.CorrectKeyForServerB(decipheredKey);
            des.DecipherFile(desPartPath, decipheredDesPath, decipheredKey, addByte);
            byte[] decipheredDesPart = File.ReadAllBytes(decipheredDesPath);

            //віднімаємо довжину M, отримуємо RSA_A(H(M))
            string rsaAHashPartPath = "2.1rsaAHashPart.txt";

            byte[] rsaAHashPart = new byte[decipheredDesPart.Length - message.Length];
            Array.Copy(decipheredDesPart, 0, rsaAHashPart, 0, decipheredDesPart.Length - message.Length);

            //файли 2rsaAHash, 2.1rsaAHashPart
            File.WriteAllBytes(rsaAHashPartPath, rsaAHashPart);

            //файли 1hashText i 1.1decipheredHashText
            string decipheredHashTextPath = "1.1decipheredHashText.txt";


            //чогось не збігається
            rsa.DecipherFileRsa(rsaAHashPartPath, decipheredHashTextPath, PublicKeyA, n_a);




            Console.WriteLine($"{PublicKeyA}, {PrivateKeyA} \n {PublicKeyB}, {PrivateKeyB} \n {n_a}, {n_b}");

            //6 , 6.1
            CompareFiles(rsaSesKeyBPath, rsaSesKeyPath);

            //5, 5.1
            CompareFiles(decipheredSesKeyPath, sesKeyPath);

            //4, 4.1
            CompareFiles(desPartPath, desPath);

            //3, 3.1
            CompareFiles(decipheredDesPath, rsaHashPlusMessagePath);

            //2, 2.1
            CompareFiles(rsaAHashPartPath, rsaAHashPath);

            //1, 1.1
            CompareFiles(decipheredHashTextPath, hashTextPath);

            #endregion

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}