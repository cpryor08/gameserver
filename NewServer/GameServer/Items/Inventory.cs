using System;
using GameServer.Network;
using System.Collections.Concurrent;

namespace GameServer.Items
{
    public class Inventory
    {
        private SocketClient Owner;
        public ConcurrentDictionary<uint, Item> Items = new ConcurrentDictionary<uint, Item>();

        public Inventory(SocketClient Owner) { this.Owner = Owner; }

        public bool TryAdd(Item Item)
        {
            if (Items.Count < 40)
            {
                if (Items.TryAdd(Item.UniqueID, Item))
                {
                    Item.Position = 0;
                    Owner.Send(Item.ToBytes);
                }
            }
            return false;
        }
        public void Remove(uint ItemUID)
        {
            Item Item;
            Items.TryRemove(ItemUID, out Item);
            Owner.Send(Packets.ToSend.ItemUsage(ItemUID, (uint)Enums.ItemUsage.RemoveInventory, 0, 0));
        }
        public bool Contains(uint ItemUID)
        {
            return Items.ContainsKey(ItemUID);
        }
        public void GetItemList(ref System.Collections.Generic.List<uint> ItemList)
        {
            foreach (uint Key in Items.Keys)
                ItemList.Add(Key);
        }
    }
}
