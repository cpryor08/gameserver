using System;
using GameServer.Network;

namespace GameServer
{
    interface Packet
    {
        int MinLength { get; }
        void Handle(byte[] Data, SocketClient Client);
    }
}
