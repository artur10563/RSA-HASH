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

            //Replace with WMI.GetPcInfo();
            Console.WriteLine($"Message: ");
            string message = Console.ReadLine();

            byte[] data = Encoding.Default.GetBytes(message);
            int messageLength = PlainTextToFile(data);

            #region Encryption
            //H(M)
            PlainTextToHash();

            //RSA_A(H(M))
            HashTextToRsaA(PrivateKeyA, n_a);
            #endregion



            var receivedBytes = 0;
            StringBuilder answer = new StringBuilder();

            //Connecting to socket
            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(Encoding.Default.GetBytes("Connected"));


            //Sending encrypted message

            //Sending length of key

            //Sending length of message

            //Отримуємо ключ від сервера байтовими блоками

            List<byte> PublicKeyBBytes = new List<byte>();
            //byte[] PublicKeyBBytes = new byte[tcpSocket.Available];
            Console.WriteLine(tcpSocket.Available);
            int offset = 0;
            do
            {
                byte[] dataChunk = new byte[256];
                receivedBytes = tcpSocket.Receive(dataChunk);


                PublicKeyBBytes.AddRange(dataChunk);

                //array.Copy(dataChunk, 0, PublicKeyBBytes, offset, receivedBytes); //кидає помилку чогось
                //offset += receivedBytes;

            } while (tcpSocket.Available > 0);

            PublicKeyB = new BigInteger(PublicKeyBBytes.ToArray());
            Console.WriteLine(PublicKeyB);

            Console.WriteLine(answer);
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();


            Console.ReadLine();
        }
    }
}
