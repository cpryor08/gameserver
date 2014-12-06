using System;
using System.Collections.Concurrent;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] ViewWarehouse(uint ActiveNPC, ConcurrentBag<Items.Item> Items)
        {
            int Len = 16 + (20 * Items.Count);
            Writer PWR = new Writer(Len);
            PWR.Fill((ushort)Len, 0);
            PWR.Fill((ushort)1102, 2);
            PWR.Fill((uint)ActiveNPC, 4);
            PWR.Fill((uint)Items.Count, 12);
            byte i = 0;
            foreach (Items.Item I in Items)
            {
                int extra = i * 20;
                PWR.Fill((uint)I.UniqueID, (16 + extra));
                PWR.Fill((uint)I.Info.ID, (20 + extra));
                PWR.Fill((byte)I.SocketOne, (25 + extra));
                PWR.Fill((byte)I.SocketTwo, (26 + extra));
                PWR.Fill((byte)I.Plus, (29 + extra));
                PWR.Fill((byte)I.Bless, (30 + extra));
                PWR.Fill((byte)I.Enchant, (32 + extra));
                i++;
            }
            return PWR.Bytes;
        }
        public static byte[] ViewWarehouse(uint ActiveNPC)
        {
            Writer PWR = new Writer(16);
            PWR.Fill((ushort)16, 0);
            PWR.Fill((ushort)1102, 2);
            PWR.Fill((uint)ActiveNPC, 4);
            return PWR.Bytes;
        }
        public static byte[] AddWHItem(uint NPC, Items.Item Item)
        {
            Writer PWR = new Writer(32);
            PWR.Fill((ushort)32, 0);
            PWR.Fill((ushort)1102, 2);
            PWR.Fill(NPC, 4);
            PWR.Fill((uint)1, 12);
            PWR.Fill(Item.UniqueID, 16);
            PWR.Fill(Item.Info.ID, 20);
            PWR.Fill(Item.SocketOne, 25);
            PWR.Fill(Item.SocketTwo, 26);
            PWR.Fill(Item.Plus, 29);
            PWR.Fill(Item.Bless, 30);
            PWR.Fill(Item.Enchant, 32);
            return PWR.Bytes;
        }
    }
}