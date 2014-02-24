using System;

namespace UDPMultiforwarder
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            try
            {
                UdpMultiFw controller = new UdpMultiFw();
                Console.WriteLine("Starting UdpMultiForwarder.");
                controller.startProcess();

                Console.WriteLine("Forwarder Process Started...");
                Console.Read();

                Console.WriteLine("Stop Forwarder Process..!");
                controller.endProcess();
                Console.WriteLine("Finish and Exit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
