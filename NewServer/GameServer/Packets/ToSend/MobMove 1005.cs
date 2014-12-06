using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] MobMove(uint UID, byte Direction)
        {
            Writer PWR = new Writer(16);
            PWR.Fill((ushort)16, 0);
            PWR.Fill((ushort)1005, 2);
            PWR.Fill((uint)UID, 4);
            PWR.Fill((byte)Direction, 8);
            PWR.Fill((byte)1, 9);
            PWR.Fill((ushort)0, 10);
            PWR.Fill((int)Environment.TickCount, 12);
            return PWR.Bytes;
        }
    }
}