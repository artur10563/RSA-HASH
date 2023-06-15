using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RSALIB;

namespace Server
{

    internal class Program
    {
        private static BigInteger GenerateKeys(out BigInteger PublicKey, out BigInteger PrivateKey)
        {
            ulong pa = 43;
            ulong qa = 59;
            //ulong pa = PrimeNumberGenerator.Generate();
            //ulong qa = PrimeNumberGenerator.Generate();

            //return n_
            return rsa.PublicKey_PrivateKey(pa, qa, out PublicKey, out PrivateKey);

        }
        private static string ReceivedHashPcInfoPath = "ReceivedHashPcInfo.dat";
        private static string EncryptedPcInfoPath = "EncryptedPcInfo.dat";

        private static RSA rsa = new RSA();
        private static HashFunction hf = new HashFunction();

        static void Main(string[] args)
        {
            #region socket init
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            #endregion

            BigInteger PublicKey;
            BigInteger PrivateKey;
            BigInteger n_ = GenerateKeys(out PublicKey, out PrivateKey);

            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(10);

            while (true)
            {
                var listener = tcpSocket.Accept();

                //Get PC info from client
                byte[] received = new byte[2048];
                int totalReceivedBytes = listener.Receive(received);


                using (MemoryStream memoryStream = new MemoryStream(received, 0, totalReceivedBytes))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    byte[] message = (byte[])formatter.Deserialize(memoryStream);

                    File.WriteAllBytes(ReceivedHashPcInfoPath, message);

                }

                //Encrypted hash
                rsa.EncryptFileRSA(ReceivedHashPcInfoPath, EncryptedPcInfoPath, PrivateKey, n_);
                byte[] encryptedHash = File.ReadAllBytes(EncryptedPcInfoPath);

                //Send encryptedHash and keys to client
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memoryStream, encryptedHash);
                    formatter.Serialize(memoryStream, PublicKey);
                    formatter.Serialize(memoryStream, n_);

                    byte[] toSend = memoryStream.ToArray();
                    listener.Send(toSend);
                }

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();

                Console.WriteLine("Sent to listener");
            }
        }
    }
}
