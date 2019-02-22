using LCRLibrary;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LCR
{
    class Program
    {
        static void Main(string[] args)
        {
            NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();
            LCRClient listener = new LCRClient();
            NetManager client = new NetManager(listener);
            listener.client = client;
            client.Start();
                       
            var server = client.Connect("localhost" /* host ip or name */, 9050 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);

            pl mpl = new pl { Name = "Bob", Tickets = 3};
            NetDataWriter dw = new NetDataWriter();

            SamplePacket samPack = new SamplePacket {  };

            Thread.Sleep(100);
            do
            {
                client.PollEvents();
                Thread.Sleep(15);

                if (server.ConnectionState == ConnectionState.Connected)
                {
                    string input = Console.ReadLine();
                    if (input == "r")
                    {
                        _netPacketProcessor.Send<pl>(server, mpl, DeliveryMethod.ReliableOrdered);
                    }
                }
            } while (server.ConnectionState == ConnectionState.Connected);

            client.Stop();
        }
    }
}
