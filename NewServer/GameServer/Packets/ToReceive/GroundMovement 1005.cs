using System;
using GameServer.Network;

namespace GameServer.Packets.ToReceive
{
    public class GroundMovement : Packet
    {
        public int MinLength { get { return 8; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            Enums.ConquerAngle Direction = (Enums.ConquerAngle)(Data[8] % 8);
            Client.Character.Walk(Direction);
            Client.Character.Screen.Send(Data, true);
            Client.Character.Map.PopulateScreen(Client);
        }
    }
}