using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;            
using System.Net.Sockets;    

namespace Cliente
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var threadNumbers = new Thread(new ThreadStart(GenerateRandomNumbers));
            threadNumbers.Start();
        }

        private static void GenerateRandomNumbers()
        {
            var sumSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Sum Server listening on port 27877
            var sumSocketAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27877);

            string numberToSend;
            byte[] sendingString;

            sumSocket.Connect(sumSocketAddress);
            Console.WriteLine("Conection Succesful...");
            var terminar = false;
            while (!terminar)
            {
                try
                {
                    var random = new Random();

                    //Random Numbers between 0 and 20
                    numberToSend = random.Next(21).ToString();
                    sendingString = Encoding.UTF8.GetBytes(numberToSend); 
                    sumSocket.Send(sendingString, 0, sendingString.Length, 0);
                    Console.WriteLine("Number Sent:"+ numberToSend);
                    
                    //Thread sleeping to send each 2miliseconds
                    Thread.Sleep(200);
                }
                catch (Exception)
                {
                    terminar = true;
                }
            }

            sumSocket.Close();
        }
    }
}