using System;
using System.Collections;
using System.Text;
using System.Net.Sockets;

namespace UDPMultiforwarder
{
    class UdpMultiSend
    {
        private ArrayList target;
        private UdpClient client;

        public UdpMultiSend(ArrayList target)
        {
            this.target = target;
            this.client = new UdpClient();
        }

        // sendData
        public void sendString(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);

            foreach(string[] t in target)
            {
                client.Send(data, data.Length, t[0], Convert.ToInt16(t[1]));

                if (Environment.UserInteractive)
                {
                    Console.WriteLine("SEND -->> " + t[0] + ":" + t[1] + " - " + message);
                }
            }
            
        }
    }
}
