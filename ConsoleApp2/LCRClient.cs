using LCRLibrary;
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
        private NetPeer Server;
        private readonly NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();
        public LCRLobby theGame;
        public pl tpl;
        private string ClientSignature
        {
            get
            {
                return $"[Client {DateTime.Now}]";
            }
        }

        public LCRClient()
        {
            _netPacketProcessor.RegisterNestedType(() => new pl());
            _netPacketProcessor.SubscribeReusable<pl, NetPeer>(ProcessPacket);
            _netPacketProcessor.SubscribeReusable<CreateGamePacket, NetPeer>(ProcessNewGame);
        }

        private void ProcessNewGame(CreateGamePacket arg1, NetPeer arg2)
        {
            if(arg1.Success)
            {
                Console.WriteLine($"{ClientSignature} game created! ID:{arg1.Id} Players:{arg1.Playernum}");
                theGame = new LCRLobby(arg1.Id, arg1.Playernum);
            }
            else
            {
                Console.WriteLine($"{ClientSignature} game failed! ID:{arg1.Id} Players:{arg1.Playernum}");

            }
        }

        public void AttachServer(NetPeer p)
        {
            Server = p;
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
            Console.WriteLine($"{ClientSignature} Received data. Processing...");
            _netPacketProcessor.ReadAllPackets(reader, peer);
        }

        public void ProcessPacket(pl thePl, NetPeer peer)
        {
            Console.WriteLine($"{thePl.Name} {thePl.Tickets}");
            tpl = thePl;
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine($"{ClientSignature} Connected to {peer.EndPoint}");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"{ClientSignature} Connection to {peer.EndPoint} died cause {disconnectInfo.Reason}");
        }

        public void SendPacket(pl thePl)
        {
            _netPacketProcessor.Send<pl>(Server, thePl, DeliveryMethod.ReliableOrdered);
        }

        public void SendCreateGameRequest(CreateGamePacket cgp)
        {
            Console.WriteLine($"{ClientSignature} creating game: ID:{cgp.Id}  Players:{cgp.Playernum}");
            _netPacketProcessor.SendNetSerializable<CreateGamePacket>(Server, cgp, DeliveryMethod.ReliableOrdered);
        }
    }

}
