using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Numerics;
using RSALIB;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;

namespace Server
{
    internal class Program
    {
        #region Encryption functions
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
        private static byte[] DecipherDesPart(ref byte[] decipheredKey, int addByte)
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
            #region socket init
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            #endregion

            BigInteger PublicKeyB;
            BigInteger PrivateKeyB;
            BigInteger n_b = GenerateKeys(out PublicKeyB, out PrivateKeyB);

            tcpSocket.Bind(tcpEndPoint);
            //10 підключень максимум
            tcpSocket.Listen(10);

            while (true)
            {
                //новий сокет для клієнта
                var listener = tcpSocket.Accept();
                Console.WriteLine("New Client connected!");

                Console.WriteLine("Sending keys...");
                //Відправляємо PublicKeyB i n_b клієнту
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memoryStream, PublicKeyB);
                    formatter.Serialize(memoryStream, n_b);
                    byte[] serializedData = memoryStream.ToArray();  //334 байти

                    listener.Send(serializedData);
                }
                Console.WriteLine("Keys sent succesfully!");


                //Отримуємо від клієнта:
                //зашифроване повідомлення,
                //довжину ключа сесії
                //довжину початкового повідомлення
                //addByte
                //PublicKeyA, n_a
                #region Receiving info from client

                byte[] final;
                int rsaBSesKeyLength = 0;
                int messageLength = 0;
                byte[] receivedData = new byte[4096]; 
                BigInteger PublicKeyA;
                BigInteger n_a;
                int addByte;

                int totalReceivedBytes = listener.Receive(receivedData);

                Console.WriteLine("Receiving info from client...");
                using (MemoryStream memoryStream = new MemoryStream(receivedData, 0, totalReceivedBytes))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    final = (byte[])formatter.Deserialize(memoryStream);
                    rsaBSesKeyLength = (int)formatter.Deserialize(memoryStream);
                    messageLength = (int)formatter.Deserialize(memoryStream);
                    PublicKeyA = (BigInteger)formatter.Deserialize(memoryStream);
                    n_a = (BigInteger)formatter.Deserialize(memoryStream);
                    addByte = (int)formatter.Deserialize(memoryStream);
                }

                #endregion

                //Decryption
                #region Decryption
                DecipherSesKey(final, rsaBSesKeyLength);


                //розшифрували  5.1 і 5 (розшифрований ключ сесії)
                rsa.DecipherFileRsa(rsaSesKeyBPath, decipheredSesKeyPath, PrivateKeyB, n_b);
                byte[] decipheredKey = File.ReadAllBytes(decipheredSesKeyPath);

                //розшифрували ключ, віднімаємо від всього переданого зашифрованого повідомлення ключ, отримуємо частину з десом
                byte[] desPart = GetDesPart(final, rsaBSesKeyLength);




                //тут помилка 
                //розшифровуємо дес, отримаємо M || RSA_A(H(M))
                //3rsaHashPlusMessage i 3.1decipheredDes
                //byte[] decipheredDesPart = DecipherDesPart(ref decipheredKey,  addByte);
                //тут помилка 
                decipheredKey = des.CorrectKeyForServerB(decipheredKey);
                des.DecipherFile(desPartPath, decipheredDesPath, decipheredKey, addByte);
                byte[] decipheredDesPart = File.ReadAllBytes(decipheredDesPath);




                //віднімаємо довжину M, отримуємо RSA_A(H(M))
                GetRsaAHashM(decipheredDesPart, messageLength);
                //файли 1hashText i 1.1decipheredHashText         
                //Розшифрований хеш, порівнюємо з початковим хешом
                rsa.DecipherFileRsa(rsaAHashPartPath, decipheredHashTextPath, PublicKeyA, n_a);
                byte[] hash = File.ReadAllBytes(decipheredHashTextPath);

                Console.WriteLine("Decrypted successfully!");
                #endregion

                //відправляємо розшифрований хеш клієнту
                Console.WriteLine("Sending hash to client...");
                listener.Send(hash);
                Console.WriteLine("Sent successfully!");

                File.Delete(rsaSesKeyBPath);
                File.Delete(decipheredSesKeyPath);
                File.Delete(desPartPath);
                File.Delete(decipheredDesPath);
                File.Delete(rsaAHashPartPath);
                File.Delete(decipheredHashTextPath);


                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
