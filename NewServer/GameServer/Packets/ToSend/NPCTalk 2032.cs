using System;
using GameServer.Network;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] NPCTalk(byte LinkBack, byte DT, string Text)
        {
            Writer PWR = new Writer(16 + Text.Length);
            PWR.Fill((ushort)(16 + Text.Length), 0);
            PWR.Fill((ushort)2032, 2);
            PWR.Fill((uint)0, 4);
            PWR.Fill((byte)LinkBack, 10);
            PWR.Fill((byte)DT, 11);
            PWR.Fill((byte)1, 12);
            PWR.Fill((byte)Text.Length, 13);
            PWR.Fill(Text, 14);
            return PWR.Bytes;
        }
        public static byte[] NPCTalk(int UK1, int ID, int LinkBack, int DT)
        {
            Writer PWR = new Writer(16);
            PWR.Fill((ushort)16, 0);
            PWR.Fill((ushort)2032, 2);
            PWR.Fill((uint)UK1, 4);
            PWR.Fill((ushort)ID, 8);
            PWR.Fill((byte)LinkBack, 10);
            PWR.Fill((byte)DT, 11);
            PWR.Fill((uint)0, 12);
            return PWR.Bytes;
        }
    }
}