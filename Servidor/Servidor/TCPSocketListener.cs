using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servidor
{
    /// <summary>
    /// Summary description for TCPSocketListener.
    /// </summary>
    public class TCPSocketListener
    {

        /// <summary>
        /// Variables that are accessed by other classes indirectly.
        /// </summary>
        private Socket m_clientSocket = null;
        private Socket _sumSocket = null;
        private bool m_stopClient = false;
        private Thread m_clientListenerThread = null;
        private List<int> _numbersList;


        /// <summary>
        /// Client Socket Listener Constructor.
        /// </summary>
        /// <param name="clientSocket"></param>
        public TCPSocketListener(Socket clientSocket)
        {
            m_clientSocket = clientSocket.Accept();
        }

        /// <summary>
        /// Client SocketListener Destructor.
        /// </summary>
        ~TCPSocketListener()
        {
            StopSocketListener();
        }

        /// <summary>
        /// Method that starts SocketListener Thread.
        /// </summary>
        public void StartSocketListener()
        {

            _sumSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var sumSocketAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27878);
            _sumSocket.Connect(sumSocketAddress);

            if (m_clientSocket != null)
            {
                m_clientListenerThread =
                    new Thread(new ThreadStart(SocketListenerThreadStart));

                m_clientListenerThread.Start();
            }
        }
        
        private void SocketListenerThreadStart()
        {
            int size = 0;
            var byteBuffer = new byte[255];
            _numbersList = new List<int>();
            
            //Sum function called from a timer 1 second periodically
            var t = new Timer(AddCurrentValues, null, 1000, 1000);

            while (!m_stopClient)
            {
                try
                {
                    byteBuffer = new byte[255];
                    size = m_clientSocket.Receive(byteBuffer, 0, byteBuffer.Length, 0);
                    ValidateAndSaveInput(ref m_stopClient, byteBuffer, size);

                }
                catch (SocketException se)
                {
                    m_stopClient = true;
                }
            }
        }

        private void AddCurrentValues(object state)
        {
            var resultSum = _numbersList.Sum();
            _numbersList = new List<int>();
            byte[] sendingString;
            sendingString = Encoding.UTF8.GetBytes(resultSum.ToString());
            _sumSocket.Send(sendingString, 0, sendingString.Length, 0);
            Console.WriteLine("Number Sent:" + resultSum);
        }


        private void ValidateAndSaveInput(ref bool m_stopClient, byte[] byteBuffer, int size)
        {
            Array.Resize(ref byteBuffer, size);
            var messageString = Encoding.UTF8.GetString(byteBuffer);
            var valueNum = 0;
            if (int.TryParse(messageString, out valueNum))
            {
                _numbersList.Add(valueNum);
            }
            else
            {
                //Close the port when invalid input
                m_stopClient = false;
            }

            Console.WriteLine("Number Sent: " + Encoding.UTF8.GetString(byteBuffer));
        }


        /// <summary>
        /// Method that stops Client SocketListening Thread.
        /// </summary>
        public void StopSocketListener()
        {
            if (m_clientSocket != null)
            {
                m_stopClient = true;
                m_clientSocket.Close();

                // Wait for one second for the the thread to stop.
                m_clientListenerThread.Join(1000);

                // If still alive; Get rid of the thread.
                if (m_clientListenerThread.IsAlive)
                {
                    m_clientListenerThread.Abort();
                }
                m_clientListenerThread = null;
                m_clientSocket = null;
            }
        }
        
    }
}
