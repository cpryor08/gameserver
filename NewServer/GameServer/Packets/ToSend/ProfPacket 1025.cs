using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] ProfPacket(uint ProfID, Profs.Prof Prof)
        {
            Writer PWR = new Writer(16);
            PWR.Fill((ushort)16, 0);
            PWR.Fill((ushort)1025, 2);
            PWR.Fill(ProfID, 4);
            PWR.Fill((uint)Prof.Level, 8);
            PWR.Fill(Prof.Experience, 12);
            return PWR.Bytes;
        }
    }
}