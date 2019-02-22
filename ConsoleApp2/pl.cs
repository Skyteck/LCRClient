using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCR
{
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
