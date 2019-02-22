using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCR
{
    public class SamplePacket : INetSerializable
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
}
