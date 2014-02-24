using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections;

namespace UDPMultiforwarder
{
    class UdpMultiForwarder
    {
        private Thread receiveThread;
        private UdpClient client;
        private UdpMultiSend senderObj;
        private ArrayList targetList;
        private bool started = false;
        private ManualResetEvent stop;
        private IPEndPoint endpoint;

        private int listenPort;

        public UdpMultiForwarder(int listenPort, ArrayList targets)
        {
            this.listenPort = listenPort;
            this.targetList = targets;
        }

        // receive thread 
        private void ReceiveData()
        {
            while (started)
            {
                var res = client.BeginReceive(iar =>
                    {
                        if(iar.IsCompleted)
                        {
                            byte[] receivedBytes = client.EndReceive(iar, ref endpoint);
                            string receivedPacket = Encoding.ASCII.GetString(receivedBytes);
                            
                            if (Environment.UserInteractive)
                            {
                                Console.WriteLine("RECV <<== " + receivedPacket);
                            }

                            senderObj.sendString(receivedPacket);
                        }
                    }, null);

                if (WaitHandle.WaitAny(new[] { stop, res.AsyncWaitHandle }) == 0)
                {
                    break;
                }

            }
        }

        public void StartReceiveData()
        {
            started = true;
            stop = new ManualResetEvent(false);

            endpoint = new IPEndPoint(IPAddress.Any, listenPort);
            client = new UdpClient(endpoint);

            senderObj = new UdpMultiSend(targetList);
            
            receiveThread = new Thread(ReceiveData);
            receiveThread.Name = "ReceiveThread";
            receiveThread.Start();
        }
        public void StopReceiveData()
        {
            stop.Set();
            started = false;
        }
    }
}
