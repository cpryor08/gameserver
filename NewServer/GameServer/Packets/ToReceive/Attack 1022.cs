using System;
using GameServer.Network;
using System.Collections.Generic;

namespace GameServer.Packets.ToReceive
{
    public class AttackPacket : Packet
    {
        public int MinLength { get { return 22; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            Client.Character.AttackHandler.Handle(Data);
        }
    }
}