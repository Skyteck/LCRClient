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
            client.DisconnectTimeout = 6000;
            pl mpl = new pl { Name = "Bob", Tickets = 3};
            NetDataWriter dw = new NetDataWriter();
            listener.tpl = mpl;
            SamplePacket samPack = new SamplePacket {  };


            Thread.Sleep(500);
            while (server.ConnectionState != ConnectionState.Connected)
            {
                int cd = 1000;
                Console.WriteLine($"[Client {DateTime.Now}] connection to localhost port 9050 failed. Trying again in {cd} ms.");

                Thread.Sleep(cd);
                server = client.Connect("localhost" /* host ip or name */, 9050 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
            }
            listener.AttachServer(server);
            while (server.ConnectionState == ConnectionState.Connected)
            {
                client.PollEvents();
                Thread.Sleep(15);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(false);
                    if(key.KeyChar == 'r')
                    {
                        listener.SendPacket(listener.tpl);
                    }
                    else if(key.KeyChar == 'c')
                    {
                        CreateGamePacket cgp = new CreateGamePacket();
                        listener.SendCreateGameRequest(cgp);
                    }
                }

                if (server.ConnectionState != ConnectionState.Connected)
                {
                    break;
                }
            } 

            client.Stop();
        }
    }
}
