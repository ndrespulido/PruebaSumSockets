using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;               
using System.Net.Sockets;       

namespace Servidor
{
    class Program
    {
        static void Main(string[] args)
        {

            var receiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Port 27877
            var receiverSocketAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27877);
            receiverSocket.Bind(receiverSocketAddress);
            receiverSocket.Listen(1);
            var mySocket = new TCPSocketListener(receiverSocket);
            mySocket.StartSocketListener();
            receiverSocket.Close();

        }
    }
}