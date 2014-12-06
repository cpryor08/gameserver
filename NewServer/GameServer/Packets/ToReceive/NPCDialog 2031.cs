using System;
using GameServer.Network;

namespace GameServer.Packets.ToReceive
{
    public class NPCDialog : Packet
    {
        public int MinLength { get { return 14; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            Client.Character.CurrentDialog.Reset();
            uint NpcUID = BitConverter.ToUInt32(Data, 4);
            if (!Client.Character.Map.NPCs.ContainsKey(NpcUID))
                return;
            Client.Character.CurrentDialog.ActiveNPC = NpcUID;
            Client.Character.CurrentDialog.Handle(Data);
        }
    }
}
