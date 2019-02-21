using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();
            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager client = new NetManager(listener);
            client.Start();
            bool die = false;
            var t = client.Connect("localhost" /* host ip or name */, 9050 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);

            listener.PeerConnectedEvent += peer => 
            {
                Console.WriteLine($"Connected to {peer.EndPoint}");
            };

            listener.PeerDisconnectedEvent += (peer, info) =>
            {
                Console.WriteLine($"Connection to {peer.EndPoint} died cause {info.Reason}");
                die = true;
            };

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
                dataReader.Recycle();
            };
            pl mpl = new pl();
            _netPacketProcessor.RegisterNestedType<pl>(() => new pl());
            //_netPacketProcessor.SubscribeReusable<SamplePacket, NetPeer>(Ontest);

            SamplePacket samPack = new SamplePacket { testPl = mpl };

            _netPacketProcessor.SendNetSerializable<SamplePacket>(client, samPack, DeliveryMethod.ReliableOrdered);
            //_netPacketProcessor.Write<pl>(dw, mpl);
            //_netPacketProcessor.Send(t, samPack, DeliveryMethod.ReliableOrdered);
            //client.SendToAll(_netPacketProcessor.Write(samPack), DeliveryMethod.ReliableOrdered);
            //client.SendToAll(dw, DeliveryMethod.ReliableOrdered);
            while (!Console.KeyAvailable)
            {
                client.PollEvents();
                Thread.Sleep(15);
                if (die)
                    return;
            }

            client.Stop();
        }

        public void Ontest(SamplePacket sp, NetPeer p)
        {
            Console.WriteLine("[Server] Received Packet!");
        }
    }

    class SamplePacket : INetSerializable
    {
        public pl testPl { get; set; }

        public void Deserialize(NetDataReader reader)
        {
            testPl.Deserialize(reader);
        }

        public void Serialize(NetDataWriter writer)
        {
            testPl.Serialize(writer);
        }
    }

    public class pl : INetSerializable
    {
        public string Name { get; set; }
        public int Tickets { get; set; }
        public delegate pl reee();
        public pl()
        {
        }
        public void Deserialize(NetDataReader reader)
        {
            Name = reader.GetString();
            Tickets = reader.GetInt();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Tickets);
            writer.Put(Name);
        }
    }
}
