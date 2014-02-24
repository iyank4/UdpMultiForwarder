using System;
using System.Collections;

namespace UDPMultiforwarder
{
    class UdpMultiFw
    {
        private int ListenPort;
        private UdpMultiForwarder fwdObj;
        private ArrayList targetList = new ArrayList();

        public void startProcess()
        {
            readConfig();
            fwdObj = new UdpMultiForwarder(ListenPort, targetList);
            
            fwdObj.StartReceiveData();
        }

        public void endProcess()
        {
            fwdObj.StopReceiveData();
        }

        private void readConfig()
        {
            // set default
            this.ListenPort = 6065;

            string line;
            string[] words1;
            string[] words2;
            int targetCounter;

            try
            {
                targetCounter = 0;
                System.IO.StreamReader f = new System.IO.StreamReader("target.txt");
                while ((line = f.ReadLine()) != null)
                {
                    if (line.Trim() != "")
                    {
                        words1 = line.Split(' ');
                        if (words1[0] == "listen")
                        {
                            this.ListenPort = Convert.ToInt16(words1[1]);
                            
                            if (Environment.UserInteractive)
                            {
                                Console.WriteLine("Set listen on port: " + this.ListenPort);
                            }
                        }
                        else if (words1[0] == "target")
                        {
                            words2 = words1[1].ToString().Split(':');
                            
                            if (Environment.UserInteractive)
                            {
                                Console.WriteLine("Target " + (targetCounter + 1) + " = IP: " + words2[0].ToString() + " port: " + words2[1]);
                            }
                            
                            this.targetList.Add(words2);
                            targetCounter++;
                        }
                    }
                }

                f.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Silakan cek kembali file target.txt");
                Console.WriteLine("");
                Console.WriteLine("Error: " + e.ToString());
                Environment.Exit(1);
            }
        }
    }
}
