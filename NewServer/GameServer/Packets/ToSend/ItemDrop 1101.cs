using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] ItemDrop(Items.FloorItem I, ushort X, ushort Y, bool Remove)
        {
            Writer PWR = new Writer(21);
            PWR.Fill((ushort)21, 0);
            PWR.Fill((ushort)1101, 2);
            PWR.Fill(I.UniqueID, 4);
            PWR.Fill(I.ItemID, 8);
            PWR.Fill(X, 12);
            PWR.Fill(Y, 14);
            PWR.Fill((byte)(Remove ? 2 : 1), 16);
            PWR.Fill(Environment.TickCount, 17);
            return PWR.Bytes;
        }
    }
}
