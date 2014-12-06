using System;
using GameServer.Network;

namespace GameServer.Packets.ToReceive
{
    public class NPCDialog2 : Packet
    {
        public int MinLength { get { return 14; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            uint NpcUID = BitConverter.ToUInt32(Data, 4);
            if (!Client.Character.Map.NPCs.ContainsKey(NpcUID))
                return;
            Client.Character.CurrentDialog.Handle(Data);
        }
    }
}
