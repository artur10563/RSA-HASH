using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
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


        private static BigInteger GenerateKeys(out BigInteger PublicKey, out BigInteger PrivateKey)
        {
            ulong pa = 43;
            ulong qa = 59;
            //ulong pa = PrimeNumberGenerator.Generate();
            //ulong qa = PrimeNumberGenerator.Generate();

            //return n_
            return rsa.PublicKey_PrivateKey(pa, qa, out PublicKey, out PrivateKey);

        }

        //Client
        #region Client functions
        private static readonly string plainTextPath = "plainText.txt";
        private static int PlainTextToFile(byte[] message)
        {
            File.WriteAllBytes(plainTextPath, message);
            return File.ReadAllBytes(plainTextPath).Length;
        }


        private static readonly string hashTextPath = "1hashText.txt";
        private static byte[] PlainTextToHash()
        {
            //4 ulongs for HashFunction
            ulong[] iv = new ulong[4]; for (int i = 0; i < iv.Length; i++) iv[i] = PrimeNumberGenerator.Generate();
            //Файл з хешом повідомлення
            byte[] hashM = hf.CreateHashCode(plainTextPath, hashTextPath, iv);
            return hashM;
        }


        private static readonly string rsaAHashPath = "2rsaAHash.txt";
        private static void HashTextToRsaA(BigInteger PrivateKeyA, BigInteger n_a)
        {
            rsa.EncryptFileRSA(hashTextPath, rsaAHashPath, PrivateKeyA, n_a);
            Console.WriteLine("rsaAHash.txt is done");
        }


        private static readonly string rsaHashPlusMessagePath = "3rsaHashPlusMessage.txt";
        private static void HashTextToRsaAPlusMessage()
        {
            byte[] M = File.ReadAllBytes(plainTextPath);
            byte[] rsaHash = File.ReadAllBytes(rsaAHashPath);
            byte[] rsaHashPlusMessage = rsaHash.Concat(M).ToArray();

            File.WriteAllBytes(rsaHashPlusMessagePath, rsaHashPlusMessage);
        }


        private static byte[] GenerateDesKeys()
        {
            //generating key
            ulong k = PrimeNumberGenerator.Generate();
            byte[] sesKey = new byte[8];
            byte[] array = des.UlongToByte((long)k);
            Array.Copy(array, 0, sesKey, 0, 4);
            k = PrimeNumberGenerator.Generate();
            array = des.UlongToByte((long)k);
            Array.Copy(array, 0, sesKey, 4, 4);

            return sesKey;
        }
        private static readonly string desPath = "4des.txt";
        private static readonly string sesKeyPath = "5sesKey.txt";
        private static void RsaAToDes(byte[] sesKey, out int addByte)
        {
            des.EncryptFile(rsaHashPlusMessagePath, desPath, sesKey, out addByte);
        }

        private static readonly string rsaSesKeyPath = "6rsaBSes.txt";
        private static readonly string finalPath = "7final.txt";

        private static byte[] DesPlusRsaBSesKey(out int rsaBLength)
        {
            byte[] d = File.ReadAllBytes(desPath);
            byte[] rb = File.ReadAllBytes(rsaSesKeyPath);  //RSA_B(sesKey)
            byte[] final = d.Concat(rb).ToArray();
            File.WriteAllBytes(finalPath, final);

            rsaBLength = rb.Length;
            return final;
        }
        #endregion

        //Server
        #region Server function
        private static readonly string rsaSesKeyBPath = "6.1rsaSesKeyB.txt";
        private static void DecipherSesKey(byte[] final, int rsaSesKeyLength)
        {
            byte[] rsaSesKeyB = new byte[rsaSesKeyLength];

            Array.Copy(final, final.Length - rsaSesKeyLength, rsaSesKeyB, 0, rsaSesKeyLength);

            //файли однакові 6 i 6.1 (рса ключа сесії)
            File.WriteAllBytes(rsaSesKeyBPath, rsaSesKeyB);
        }


        private static readonly string decipheredSesKeyPath = "5.1decipheredKey.txt";


        private static readonly string desPartPath = "4.1desPart.txt";
        private static byte[] GetDesPart(byte[] final, int rsaSesKeyLength)
        {
            byte[] desPart = new byte[final.Length - rsaSesKeyLength];
            Array.Copy(final, 0, desPart, 0, final.Length - rsaSesKeyLength);

            //Деси сходяться, див файли 4.1desPart.txt i 4des.txt
            File.WriteAllBytes(desPartPath, desPart);
            return desPart;
        }


        private static readonly string decipheredDesPath = "3.1decipheredDes.txt";
        private static byte[] DecipherDesPart(byte[] decipheredKey, int addByte)
        {
            decipheredKey = des.CorrectKeyForServerB(decipheredKey);
            des.DecipherFile(desPartPath, decipheredDesPath, decipheredKey, addByte);
            byte[] decipheredDesPart = File.ReadAllBytes(decipheredDesPath);
            return decipheredDesPart;
        }


        private static readonly string rsaAHashPartPath = "2.1rsaAHashPart.txt";
        private static void GetRsaAHashM(byte[] decipheredDesPart, int messageLength)
        {
            //віднімаємо довжину M, отримуємо RSA_A(H(M))
            byte[] rsaAHashPart = new byte[decipheredDesPart.Length - messageLength];
            Array.Copy(decipheredDesPart, 0, rsaAHashPart, 0, decipheredDesPart.Length - messageLength);

            //файли 2rsaAHash, 2.1rsaAHashPart
            File.WriteAllBytes(rsaAHashPartPath, rsaAHashPart);
        }

        private static readonly string decipheredHashTextPath = "1.1decipheredHashText.txt";
        #endregion

        static void Main(string[] args)
        {
            #region Client
            //Create test text file 
            byte[] message = Encoding.Default.GetBytes("test message");
            int messageLength = message.Length;

            //H(M)
            PlainTextToHash();

            //A Keys
            #region A Keys
            BigInteger PublicKeyA;
            BigInteger PrivateKeyA;
            BigInteger n_a = GenerateKeys(out PublicKeyA, out PrivateKeyA);
            #endregion

            //RSA_A(H(M))
            HashTextToRsaA(PrivateKeyA, n_a);

            //M || RSA_A(H(M))
            HashTextToRsaAPlusMessage();

            //DES(M || RSA_A(H(M))), Session key
            #region DES(M || RSA_A(H(M))), Skey
            byte[] sesKey = GenerateDesKeys();
            int addByte = 0;
            RsaAToDes(sesKey, out addByte);

            //save session key
            File.WriteAllBytes(sesKeyPath, sesKey);

            #endregion

            //B Keys
            #region B Keys

            BigInteger PublicKeyB;
            BigInteger PrivateKeyB;
            BigInteger n_b = GenerateKeys(out PublicKeyB, out PrivateKeyB);


            #endregion

            //RSA_B(sesKey)
            #region RSA_B(sesKey
            rsa.EncryptFileRSA(sesKeyPath, rsaSesKeyPath, PublicKeyB, n_b);

            #endregion

            //DES(M || RSA_A(H(M))) || RSA_B(sesKey)
            #region DES(M || RSA_A(H(M))) || RSA_B(sesKey)

            int rsaBSesKeyLength;
            byte[] final = DesPlusRsaBSesKey(out rsaBSesKeyLength);

            #endregion

            #endregion

            #region Server


            int rsaSesKeyLength = rsaBSesKeyLength; //нова змінна, бо ми передаємо тільки довжину ключа сесії, а не сам ключ 
            DecipherSesKey(final, rsaSesKeyLength);


            //розшифрували  5.1 і 5 (розшифрований ключ сесії)
            rsa.DecipherFileRsa(rsaSesKeyBPath, decipheredSesKeyPath, PrivateKeyB, n_b);
            byte[] decipheredKey = File.ReadAllBytes(decipheredSesKeyPath);

            //розшифрували ключ, віднімаємо від всього переданого зашифрованого повідомлення ключ, отримуємо частину з десом
            byte[] desPart = GetDesPart(final, rsaBSesKeyLength);

            //розшифровуємо дес, отримаємо M || RSA_A(H(M))
            //3rsaHashPlusMessage i 3.1decipheredDes
            byte[] decipheredDesPart = DecipherDesPart(decipheredKey, addByte);


            //віднімаємо довжину M, отримуємо RSA_A(H(M))

            GetRsaAHashM(decipheredDesPart, messageLength);
            //файли 1hashText i 1.1decipheredHashText         
            //Розшифрований хеш, порівнюємо з початковим хешом
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