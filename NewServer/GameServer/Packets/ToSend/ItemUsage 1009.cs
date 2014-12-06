using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] ItemUsage(uint UniqueID, uint ID, uint dwParam, uint dwExtraInfo)
        {
            Writer PWR = new Writer(24);
            PWR.Fill((ushort)24, 0);
            PWR.Fill((ushort)1009, 2);
            PWR.Fill(UniqueID, 4);
            PWR.Fill(dwParam, 8);
            PWR.Fill(ID, 12);
            PWR.Fill(Environment.TickCount, 16);
            PWR.Fill(dwExtraInfo, 20);
            return PWR.Bytes;
        }
    }
}