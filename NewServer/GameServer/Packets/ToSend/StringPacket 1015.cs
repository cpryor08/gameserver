using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] StringPacket(uint UID, byte Type, byte Number, string name)
        {
            Writer PWR = new Writer(13 + name.Length);
            PWR.Fill((ushort)(13 + name.Length), 0);
            PWR.Fill((ushort)1015, 2);
            PWR.Fill(UID, 4);
            PWR.Fill(Type, 8);
            PWR.Fill(Number, 9);
            PWR.Fill((byte)name.Length, 10);
            PWR.Fill(name, 11);
            return PWR.Bytes;
        }
    }
}