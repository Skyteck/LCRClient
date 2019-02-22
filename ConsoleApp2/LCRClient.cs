using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LCR
{
    class LCRClient : INetEventListener
    {
        public NetManager client;
        private readonly NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();

        public LCRClient()
        {
            _netPacketProcessor.RegisterNestedType(() => new pl());
            _netPacketProcessor.SubscribeReusable<pl, NetPeer>(ProcessPacket);
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            Console.WriteLine($"[Client {DateTime.Now}] Received data. Processing...");
            //_netPacketProcessor.ReadAllPackets(dataReader, fromPeer); // LiteNetLib.Utils.ParseException: 'Undefined packet in NetDataReader'
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine($"[Client] Connected to {peer.EndPoint}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"[Client] Connection to {peer.EndPoint} died cause {disconnectInfo.Reason}");
            //die = true;
        }

        public void ProcessPacket(pl thePl, NetPeer peer)
        {

        }
    }

}
