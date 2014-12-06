using System;
using System.IO;
using System.Collections.Concurrent;
namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadShops()
        {
            IniFile I = new IniFile("C:/db/shop.dat");
            int ShopAmount = I.ReadInt32("Header", "Amount");
            for (int i = 0; i < ShopAmount; i++)
            {
                NPCs.NpcShop S = new NPCs.NpcShop();
                S.ShopID = I.ReadUInt32("Shop" + i.ToString(), "ID");
                S.Type = I.ReadByte("Shop" + i.ToString(), "Type");
                S.MoneyType = I.ReadByte("Shop" + i.ToString(), "MoneyType");
                S.ItemCount = I.ReadByte("Shop" + i.ToString(), "ItemAmount");
                S.Items = new ConcurrentBag<uint>();
                for (int e = 0; e < S.ItemCount; e++)
                    S.Items.Add(I.ReadUInt32("Shop" + i.ToString(), "Item" + e.ToString()));
                Kernel.NpcShops.TryAdd(S.ShopID, S);
            }
            I.Close();
        }
    }
}