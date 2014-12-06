using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] SpellPacket(ushort ID, ushort Level, uint Experience)
        {
            Writer PWR = new Writer(12);
            PWR.Fill((ushort)12, 0);
            PWR.Fill((ushort)1103, 2);
            PWR.Fill(Experience, 4);
            PWR.Fill(ID, 8);
            PWR.Fill(Level, 10);
            return PWR.Bytes;
        }
    }
}