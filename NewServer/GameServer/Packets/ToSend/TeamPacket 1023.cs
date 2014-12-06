using System;
namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] TeamPacket(uint UniqueID, Enums.TeamPacket Mode)
        {
            Writer PWR = new Writer(12);
            PWR.Fill((ushort)12, 0);
            PWR.Fill((ushort)1023, 2);
            PWR.Fill((uint)Mode, 4);
            PWR.Fill(UniqueID, 8);
            return PWR.Bytes;
        }
    }
}