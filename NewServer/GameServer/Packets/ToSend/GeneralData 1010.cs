using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] GeneralData(uint UniqueID, uint Value1, ushort Value2, ushort Value3, ushort Value4, Enums.GeneralData Type)
        {
            Writer PWR = new Writer(28);
            PWR.Fill((ushort)28, 0);
            PWR.Fill((ushort)1010, 2);
            PWR.Fill((int)Environment.TickCount, 4);
            PWR.Fill((uint)UniqueID, 8);
            PWR.Fill((uint)Value1, 12);
            PWR.Fill((ushort)Value2, 16);
            PWR.Fill((ushort)Value3, 18);
            PWR.Fill((ushort)Value4, 20);
            PWR.Fill((ushort)Type, 22);
            PWR.Fill((uint)0, 24);
            return PWR.Bytes;
        }
    }
}