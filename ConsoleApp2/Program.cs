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
            client.Connect("localhost" /* host ip or name */, 9050 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
                dataReader.Recycle();
            };
            pl mpl = new pl("Bob", 3);
            _netPacketProcessor.RegisterNestedType<pl>(pl.reee);
            while (!Console.KeyAvailable)
            {
                client.PollEvents();
                Thread.Sleep(15);
            }

            client.Stop();
        }
    }

    struct spl : INetSerializable
    {
        public string Name { get; set; }
        public int Tickets { get; set; }
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

    public class pl : INetSerializable
    {
        spl thespl;
        public string Name { get; set; }
        public int Tickets { get; set; }
        public delegate pl reee();
        public pl(string name, int tickets)
        {
            Name = name;
            Tickets = tickets;
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
