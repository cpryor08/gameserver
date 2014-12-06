using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] MessagePacket(string Message, string From, string To, Enums.ChatType ChatType)
        {
            int Length = 32 + From.Length + To.Length + Message.Length + 1;
            Writer PWR = new Writer(Length);
            PWR.Fill((ushort)Length, 0);
            PWR.Fill((ushort)1004, 2);
            PWR.Fill((uint)0, 4);
            PWR.Fill((uint)ChatType, 8);
            PWR.Fill((byte)4, 24);
            PWR.Fill((byte)From.Length, 25);
            PWR.Fill(From, 26);
            PWR.Fill((byte)To.Length, 26 + From.Length);
            PWR.Fill(To, 27 + From.Length);
            PWR.Fill((byte)Message.Length, 28 + From.Length + To.Length);
            PWR.Fill(Message, 29 + From.Length + To.Length);
            return PWR.Bytes;
        }
    }
}
