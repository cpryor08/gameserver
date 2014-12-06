using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections.Concurrent;
using GameServer.Network;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadItems(SocketClient Client)
        {
            string file = "C:/db/Players/" + Client.UniqueID + "/itemkeys.dat";
            if (File.Exists(file))
            {
                using (BinaryReader br = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    uint count = br.ReadUInt32();
                    for (uint i = 0; i < count; i++)
                        LoadItem(Client, br.ReadUInt32());
                    br.Close();
                }
                File.Delete(file);
            }
            else
            {
                using (DataTable dt = Database.ItemDB.GetDataTable("SELECT `UniqueID`, `ItemID`, `Plus`, `SocketOne`, `SocketTwo`, `Enchant`, `Position`, `Durability` FROM `items` WHERE `OwnerUID`=" + Client.UniqueID))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        Items.Item item = new Items.Item(Convert.ToUInt32(dr.ItemArray[1]));
                        item.UniqueID = Convert.ToUInt32(dr.ItemArray[0]);
                        item.Plus = Convert.ToByte(dr.ItemArray[2]);
                        item.SocketOne = Convert.ToByte(dr.ItemArray[3]);
                        item.SocketTwo = Convert.ToByte(dr.ItemArray[4]);
                        item.Enchant = Convert.ToByte(dr.ItemArray[5]);
                        item.Position = Convert.ToByte(dr.ItemArray[6]);
                        item.Durability = Convert.ToUInt16(dr.ItemArray[7]);
                        item.Loaded = true;
                        if (item.Position == 0)
                            Client.Character.Inventory.TryAdd(item);
                        else if (item.Position < 10)
                            Client.Character.Equipment.Equip(item, item.Position);
                        else
                        {
                            if (!Client.Character.Warehouses.ContainsKey(item.Position))
                                Client.Character.Warehouses.TryAdd(item.Position, new ConcurrentBag<Items.Item>());
                            byte spaces = 20;
                            if (item.Position == 10)
                                spaces = 40;
                            if (Client.Character.Warehouses[item.Position].Count >= spaces)
                                Console.WriteLine("Inventory full, yet adding items. Something went wrong.");
                            else
                                Client.Character.Warehouses[item.Position].Add(item);
                        }
                    }
                }
            }
        }
        private static void LoadItem(SocketClient Client, uint ItemUID)
        {
            using (DataTable dt = Database.ItemDB.GetDataTable("SELECT `ItemID`, `Plus`, `SocketOne`, `SocketTwo`, `Enchant`, `Position`, `Durability` FROM `items` WHERE `UniqueID`=" + ItemUID))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Items.Item item = new Items.Item(Convert.ToUInt32(dr.ItemArray[0]));
                    item.UniqueID = ItemUID;
                    item.Plus = Convert.ToByte(dr.ItemArray[1]);
                    item.SocketOne = Convert.ToByte(dr.ItemArray[2]);
                    item.SocketTwo = Convert.ToByte(dr.ItemArray[3]);
                    item.Enchant = Convert.ToByte(dr.ItemArray[4]);
                    item.Position = Convert.ToByte(dr.ItemArray[5]);
                    item.Durability = Convert.ToUInt16(dr.ItemArray[6]);
                    item.Loaded = true;
                    if (item.Position == 0)
                        Client.Character.Inventory.TryAdd(item);
                    else if (item.Position < 10)
                        Client.Character.Equipment.Equip(item, item.Position);
                    else
                    {
                        if (!Client.Character.Warehouses.ContainsKey(item.Position))
                            Client.Character.Warehouses.TryAdd(item.Position, new ConcurrentBag<Items.Item>());
                        byte spaces = 20;
                        if (item.Position == 10)
                            spaces = 40;
                        if (Client.Character.Warehouses[item.Position].Count >= spaces)
                            Console.WriteLine("Inventory full, yet adding items. Something went wrong.");
                        else
                            Client.Character.Warehouses[item.Position].Add(item);
                    }
                }
            }
        }
        public static void GenerateItemKeyFile(SocketClient Client)
        {
            List<uint> ItemUIDs = new List<uint>();
            Client.Character.Equipment.GetItemList(ref ItemUIDs);
            Client.Character.Inventory.GetItemList(ref ItemUIDs);
            foreach (ConcurrentBag<Items.Item> WH in Client.Character.Warehouses.Values)
                foreach (Items.Item item in WH)
                    ItemUIDs.Add(item.UniqueID);
            if (!Directory.Exists("C:/db/Players/" + Client.UniqueID))
                Directory.CreateDirectory("C:/db/Players/" + Client.UniqueID);
            using (BinaryWriter bw = new BinaryWriter(File.Open("C:/db/Players/" + Client.UniqueID + "/itemkeys.dat", FileMode.CreateNew)))
            {
                bw.Write((uint)ItemUIDs.Count);
                foreach (uint ItemUID in ItemUIDs)
                    bw.Write(ItemUID);
            }
            ItemUIDs.Clear();
        }
        public static bool ContainsItem(uint UniqueID)
        {
            return Convert.ToByte(Database.ItemDB.ExecuteScalar("SELECT COUNT(*) FROM `items` WHERE `UniqueID`=" + UniqueID)) == 1;
        }
        public static uint InsertItem(Items.Item Item)
        {
            return Convert.ToUInt32(Database.ItemDB.ExecuteScalar("INSERT INTO `items` (`ItemID`, `OwnerUID`, `Plus`, `SocketOne`, `SocketTwo`, `Enchant`, `Bless`, `Position`, `Durability`, `Locked`) VALUES ('" + Item.Info.ID + "', '" + Item.OwnerUID + "', '" + Item.Plus + "', '" + Item.SocketOne + "', '" + Item.SocketTwo + "', '" + Item.Enchant + "', '" + Item.Bless + "', '" + Item.Position + "', '" + Item.Durability + "', '" + Item.Locked + "'); SELECT last_insert_rowid()"));
        }
    }
}