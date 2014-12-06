using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] SyncPacket(uint UniqueID, Enums.SyncType SyncType, uint Value)
        {
            Writer PWR = new Writer(20);
            PWR.Fill((ushort)20, 0);
            PWR.Fill((ushort)1017, 2);
            PWR.Fill(UniqueID, 4);
            PWR.Fill((uint)1, 8);
            PWR.Fill((uint)SyncType, 12);
            PWR.Fill(Value, 16);
            return PWR.Bytes;
        }
        public static byte[] SyncPacket(uint UniqueID, Enums.SyncType SyncType, long Value)
        {
            Writer PWR = new Writer(24);
            PWR.Fill((ushort)24, 0);
            PWR.Fill((ushort)1017, 2);
            PWR.Fill(UniqueID, 4);
            PWR.Fill((uint)1, 8);
            PWR.Fill((uint)SyncType, 12);
            PWR.Fill(Value, 16);
            return PWR.Bytes;
        }
    }
}