using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace Server
{
    class Program
    {
        const int port = 8007;
        const string server_ip = "127.0.0.1";
        public static IPAddress andress = IPAddress.Parse(server_ip);
        public static TcpListener serverSocket = new TcpListener(andress, port);
        static void Main(string[] args)
        {
            serverSocket.Start();
            while (true) // ждем подключений клиентов
            {
                TcpClient client = serverSocket.AcceptTcpClient();
                // Каждого подключившего клиента обрабатываем в отдельном потоке
                Thread Thread = new Thread(new ParameterizedThreadStart(Method));
                Thread.Start(client);
            }
        }
        static void Method(Object client2)
        {
            while (true)
            {
                TcpClient client = (TcpClient)client2;
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine(dataReceived);
                nwStream.Write(buffer, 0, bytesRead);
                nwStream.Flush();
            }

        }
    }
}

