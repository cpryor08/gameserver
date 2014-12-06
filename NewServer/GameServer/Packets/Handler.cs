using System;
using GameServer.Network;
using System.Data;
using System.Collections.Generic;

namespace GameServer.Packets
{
    public class Handler
    {
        private static Dictionary<ushort, Packet> packetHandlers;
        public static void Init()
        {
            packetHandlers = new Dictionary<ushort, Packet>();
            packetHandlers.Add(1001, new Packets.ToReceive.NewCharacter());
            packetHandlers.Add(1004, new Packets.ToReceive.ChatPacket());
            packetHandlers.Add(1005, new Packets.ToReceive.GroundMovement());
            packetHandlers.Add(1009, new Packets.ToReceive.ItemPacket());
            packetHandlers.Add(1010, new Packets.ToReceive.GeneralData());
            packetHandlers.Add(1022, new Packets.ToReceive.AttackPacket());
            packetHandlers.Add(1052, new Packets.ToReceive.GameConnect());
            packetHandlers.Add(1102, new Packets.ToReceive.WarehousePacket());
            packetHandlers.Add(2031, new Packets.ToReceive.NPCDialog());
            packetHandlers.Add(2032, new Packets.ToReceive.NPCDialog2());
        }
        public static void Handle(byte[] Data, SocketClient Client)
        {
            Client.Crypto.Decrypt(ref Data);
            if (Data.Length < 4)
            {
                Client.Disconnect();
                return;
            }
            ushort PacketID = BitConverter.ToUInt16(Data, 2);
            Packet handler;
            if (packetHandlers.TryGetValue(PacketID, out handler))
            {
                if (Data.Length >= handler.MinLength)
                    handler.Handle(Data, Client);
                else
                    Client.Disconnect();
            }
            else
                Console.WriteLine("Missing PacketID: {0}", PacketID);
        }
    }
}
