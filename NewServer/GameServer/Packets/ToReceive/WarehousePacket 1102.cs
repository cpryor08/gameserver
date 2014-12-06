using System;
using GameServer.Network;
using System.Collections.Concurrent;

namespace GameServer.Packets.ToReceive
{
    public class WarehousePacket : Packet
    {
        public int MinLength { get { return 8; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            byte WHKey;
            if (!Kernel.WarehouseKeys.TryGetValue(Client.Character.MapID, out WHKey))
                return;
            byte Type = Data[8];
            switch(Type)
            {
                case 0:
                    {
                        ConcurrentBag<Items.Item> WHItems;
                        if (Client.Character.Warehouses.TryGetValue(Client.Character.MapID, out WHItems))
                            Client.Send(Packets.ToSend.ViewWarehouse(Client.Character.CurrentDialog.ActiveNPC, WHItems));
                        else
                            Client.Send(Packets.ToSend.ViewWarehouse(Client.Character.CurrentDialog.ActiveNPC));
                        break;
                    }
                case 1:
                    {
                        uint ItemUID = BitConverter.ToUInt32(Data, 12);
                        Items.Item Item;
                        if (!Client.Character.Inventory.Items.TryGetValue(ItemUID, out Item))
                            return;
                        ConcurrentBag<Items.Item> WHItems;
                        if (!Client.Character.Warehouses.TryGetValue(Client.Character.MapID, out WHItems))
                        {
                            WHItems = new ConcurrentBag<Items.Item>();
                            Client.Character.Warehouses.TryAdd(Client.Character.MapID, WHItems);
                        }
                        byte MaxSlots = 20;
                        if (Client.Character.MapID == 1036)
                            MaxSlots = 40;
                        if (WHItems.Count >= MaxSlots)
                            return;
                        Client.Character.Inventory.Remove(ItemUID);
                        Item.Position = WHKey;
                        WHItems.Add(Item);
                        if (WHItems.Count == 21)
                            Client.Send(Packets.ToSend.AddWHItem(Client.Character.CurrentDialog.ActiveNPC, Item));
                        Client.Send(Packets.ToSend.ViewWarehouse(Client.Character.CurrentDialog.ActiveNPC, WHItems));
                        break;
                    }
                case 2:
                    {
                        uint ItemUID = BitConverter.ToUInt32(Data, 12);
                        ConcurrentBag<Items.Item> WHItems;
                        if (!Client.Character.Warehouses.TryGetValue(Client.Character.MapID, out WHItems))
                            return;
                        foreach (Items.Item I in WHItems)
                        {
                            if (I.UniqueID == ItemUID)
                            {
                                Items.Item removedItem;
                                if (WHItems.TryTake(out removedItem))
                                {
                                    Client.Send(Packets.ToSend.ViewWarehouse(Client.Character.CurrentDialog.ActiveNPC, WHItems));
                                    removedItem.Position = 0;
                                    Client.Character.Inventory.TryAdd(removedItem);
                                    Client.Send(removedItem.ToBytes);
                                }
                                return;
                            }
                        }
                        break;
                    }
                default: Console.WriteLine("Missing Warehouse Packet: " + Type); break;
            }
        }
    }
}
