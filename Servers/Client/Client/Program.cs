using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using RSALIB;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

//Socket B is Client, send length of message
namespace Client
{
    internal class Program
    {

        #region Encryption Functions

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
        private static string DictToString<K, V>(Dictionary<K, V> d)
        {
            if (d.Count == 0) return "";

            StringBuilder sb = new StringBuilder();
            foreach (var item in d)
            {
                string key = item.Key.ToString();
                string value = item.Value == null ? "" : item.Value.ToString();

                sb.Append(key + " : " + value + "\n");
            }

            return sb.ToString().Trim();
        }
        static void Main(string[] args)
        {
            
            #region socket init
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            #endregion

            //Client keys
            BigInteger PublicKeyA;
            BigInteger PrivateKeyA;
            BigInteger n_a = GenerateKeys(out PublicKeyA, out PrivateKeyA);

            //Server keys
            BigInteger PublicKeyB;
            BigInteger n_b;


            //Connecting to socket
            tcpSocket.Connect(tcpEndPoint);


            //Отримуємо відкриті ключі B
            byte[] receivedData = new byte[512]; //Сервер відправляє в середньому 334 байти

            int totalReceivedBytes = tcpSocket.Receive(receivedData);
            #region Receiving B keys

            using (MemoryStream memoryStream = new MemoryStream(receivedData, 0, totalReceivedBytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                PublicKeyB = (BigInteger)formatter.Deserialize(memoryStream);
                n_b = (BigInteger)formatter.Deserialize(memoryStream);
            }
            Console.WriteLine("Received Server Keys...");
            #endregion



            #region Encryption
            //Replace with WMI.GetPcInfo();
            //Console.WriteLine($"Message: ");
            //string message = Console.ReadLine();
            string message = DictToString(RSALIB.WMI.GetProcessorInfo());

            byte[] data = Encoding.Default.GetBytes(message);
            int messageLength = PlainTextToFile(data);

            Console.WriteLine("Started Encryption...");

            //H(M)
            byte[] hash = PlainTextToHash();

            //RSA_A(H(M))
            HashTextToRsaA(PrivateKeyA, n_a);

            //M || RSA_A(H(M))
            HashTextToRsaAPlusMessage();

            //DES(M || RSA_A(H(M))), Session key         
            byte[] sesKey = GenerateDesKeys();
            int addByte = 0;
            RsaAToDes(sesKey, out addByte);
            //save session key
            File.WriteAllBytes(sesKeyPath, sesKey);

            //RSA_B(sesKey)
            rsa.EncryptFileRSA(sesKeyPath, rsaSesKeyPath, PublicKeyB, n_b);

            // DES(M || RSA_A(H(M))) || RSA_B(sesKey)
            //Фінальне повідомлення, йде на сервер
            int rsaBSesKeyLength;
            byte[] final = DesPlusRsaBSesKey(out rsaBSesKeyLength);

            Console.WriteLine("Encrypting is done!");
            #endregion

            //Надсилаємо  зашифроване повідомлення, довжину ключа сесії та довжину повідомлення.
            //І PublicKeyA, n_a, addByte
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, final);            //зашифроване повідомлення
                formatter.Serialize(memoryStream, rsaBSesKeyLength); //довжина ключа сесії
                formatter.Serialize(memoryStream, data.Length);      //довжина початкового повідомлення
                formatter.Serialize(memoryStream, PublicKeyA);       //PublicKeyA
                formatter.Serialize(memoryStream, n_a);              //n_a
                formatter.Serialize(memoryStream, addByte);          //addByte

                byte[] serializedData = memoryStream.ToArray();

                tcpSocket.Send(serializedData);
            }



            //wait until server send's response
            //Receive hash from server

            while (tcpSocket.Available == 0)
            {

            }
            Thread.Sleep(3000);

            byte[] hashFromServer = new byte[2048];
            int size = tcpSocket.Receive(hashFromServer);
            Array.Resize(ref hashFromServer, size);





            //Порівнюємо
            if (hashFromServer.SequenceEqual(hash))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed!");
                Console.ResetColor();
            }

            File.Delete(plainTextPath);             // plain
            File.Delete(hashTextPath);              //1
            File.Delete(rsaAHashPath);              //2
            File.Delete(rsaHashPlusMessagePath);    //3
            File.Delete(desPath);                   //4
            File.Delete(sesKeyPath);                //5
            File.Delete(rsaSesKeyPath);             //6
            File.Delete(finalPath);                 //7



            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();


            Console.ReadLine();
        }
    }
}
