using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace SumWebApp
{
    public partial class SumGraph : System.Web.UI.Page
    {
        private static Socket _receiverSocket;
        private static string currentNumber;

        protected void Page_Load(object sender, EventArgs e)
        {

            lblSumServerValue.Text = "50";
        }

        protected void tmrUpdate_Tick(object sender, EventArgs e)
        {

            lblSumServerValue.Text = currentNumber;
        }


        protected void btnStart_Click(object sender, EventArgs e)
        {
            StartListening();
        }

        public void StartListening()
        {

            _receiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var receiverSocketAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27878);
            _receiverSocket.Bind(receiverSocketAddress);
            
            
            var newThread = new Thread(new ThreadStart(SocketListenerThreadStart));
            //start thread
            newThread.Start();

            //mySocket.StartSocketListener();
            //receiverSocket.Close();
        }

        private void SocketListenerThreadStart()
        {
            int size = 0;
            var byteBuffer = new byte[255];
            _receiverSocket.Listen(1);
            Socket listenerSocket = _receiverSocket.Accept();

            var stopClient = false;

            while (!stopClient)
            {
                try
                {
                    byteBuffer = new byte[255];

                    size = listenerSocket.Receive(byteBuffer, 0, byteBuffer.Length, 0);
                    Array.Resize(ref byteBuffer, size);
                    var messageString = Encoding.UTF8.GetString(byteBuffer);
                    currentNumber = messageString;


                }
                catch (SocketException se)
                {
                    stopClient = true;
                }
            }
        }


    }


}