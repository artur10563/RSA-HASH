using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SocketServerB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            Console.WriteLine($"Message: ");
            var data = Encoding.Default.GetBytes(Console.ReadLine());

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            var dataChunk = new byte[256];
            var receivedBytes = 0;
            StringBuilder answer = new StringBuilder();

            do
            {
                receivedBytes = tcpSocket.Receive(dataChunk);
                answer.Append(Encoding.Default.GetString(dataChunk, 0, receivedBytes));

            } while (tcpSocket.Available > 0);

            Console.WriteLine(answer);
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();


            Console.ReadLine();
        }
    }
}
