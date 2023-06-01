using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SocketsServerA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);

            //10 підключень максимум
            tcpSocket.Listen(10);

            while (true)
            {
                //новий сокет для клієнта
                var listener = tcpSocket.Accept();
                var dataChunk = new byte[256];
                var receivedBytes = 0;
                StringBuilder data = new StringBuilder();

                do
                {
                    receivedBytes = listener.Receive(dataChunk);
                    data.Append(Encoding.Default.GetString(dataChunk, 0, receivedBytes));

                } while (listener.Available > 0);

                Console.WriteLine(data);
                listener.Send(Encoding.Default.GetBytes("Sent successfully"));

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }

        }
    }
}
