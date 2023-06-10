using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Numerics;
using RSALIB;

namespace Server
{
    internal class Program
    {
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

                byte[] pk_b = PublicKeyB.ToByteArray();
                byte[] n_bb = n_b.ToByteArray();
                listener.Send(pk_b);
                //listener.Send(n_bb);
                Console.WriteLine(PublicKeyB);


                var dataChunk = new byte[256];
                var receivedBytes = 0;
                StringBuilder data = new StringBuilder();

                do
                {
                    receivedBytes = listener.Receive(dataChunk);
                    data.Append(Encoding.Default.GetString(dataChunk, 0, receivedBytes));

                } while (listener.Available > 0);

                Console.WriteLine(data);

                //Відправити повідомлення клієнту
                listener.Send(Encoding.Default.GetBytes("Sent successfully"));

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }

        }
    }
}
