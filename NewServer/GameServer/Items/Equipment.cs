using System;
using GameServer.Network;
using System.Collections.Concurrent;

namespace GameServer.Items
{
    public class Equipment
    {
        private SocketClient Owner;
        private object syncroot = new object();
        private Item[] equipArray = new Item[10];

        public Equipment(SocketClient Owner) { this.Owner = Owner; }

        public bool Equip(Item item, byte ToPosition)
        {
            bool r = false;
            lock (syncroot)
            {
                if (equipArray[item.Position] == null)
                {
                    item.Position = ToPosition;
                    equipArray[item.Position] = item;
                    #region Load Item Stats
                    if (item.Durability > 0)
                    {
                        if (item.Position == Enums.ItemPosition.LeftHand)
                        {
                            Owner.Character.MinAttack += item.Info.MinAttack / 2;
                            Owner.Character.MaxAttack += item.Info.MaxAttack / 2;
                            Owner.Character.MagicAttack += item.Info.MagicAttack / 2;
                        }
                        else
                        {
                            Owner.Character.MinAttack += item.Info.MinAttack;
                            Owner.Character.MaxAttack += item.Info.MaxAttack;
                            Owner.Character.MagicAttack += item.Info.MagicAttack;
                        }
                        Owner.Character.MDefenseByPct += item.Info.MagicDefense;
                        Owner.Character.PDefense += item.Info.Defense;
                        Owner.Character.Dodge += item.Info.Dodge;
                        Owner.Character.BonusHPByItems += item.Info.VitGive;
                        Owner.Character.BonusHPByItems += item.Enchant;
                        if (item.Plus > 0)
                        {
                            if (item.Position == Enums.ItemPosition.LeftHand)
                            {
                                Owner.Character.MinAttack += item.PlusInfo.MinAtk / 2;
                                Owner.Character.MaxAttack += item.PlusInfo.MaxAtk / 2;
                                Owner.Character.MagicAttack += item.PlusInfo.MAtk / 2;
                            }
                            else
                            {
                                Owner.Character.MinAttack += item.PlusInfo.MinAtk;
                                Owner.Character.MaxAttack += item.PlusInfo.MaxAtk;
                                Owner.Character.MagicAttack += item.PlusInfo.MAtk;
                            }
                            Owner.Character.MDefenseByVal += item.PlusInfo.MDef;
                            Owner.Character.PDefense += item.PlusInfo.Defense;
                            Owner.Character.Dodge += item.PlusInfo.Dodge;
                            Owner.Character.BonusHPByItems += item.PlusInfo.HP;
                        }
                        if (item.Position == Enums.ItemPosition.RightHand)
                            Owner.Character.AttackRange = item.Info.Range;
                    }
                    #endregion

                    Owner.Send(item.ToBytes);
                    item.Mode = Enums.ItemMode.UpdateItem;
                    Owner.Send(item.ToBytes);
                    item.Mode = Enums.ItemMode.Default;
                    Owner.Character.Screen.Send(Owner.Character.ToBytes(), false);
                    r = true;
                    item.Save();
                }
            }
            return r;
        }
        public bool Uneqip(uint ItemUID)
        {
            if (Owner.Character.Inventory.Items.Count >= 40)
                return false;
            lock (syncroot)
            {
                Items.Item item = null;
                for (int i = 0; i < equipArray.Length; i++)
                    if (equipArray[i] != null)
                        if (equipArray[i].UniqueID == ItemUID)
                        { item = equipArray[i]; break; }
                if (item == null)
                    return false;

                #region Unload Item Stats
                if (item.Durability > 0)
                {
                    if (item.Position == Enums.ItemPosition.LeftHand)
                    {
                        Owner.Character.MinAttack -= item.Info.MinAttack / 2;
                        Owner.Character.MaxAttack -= item.Info.MaxAttack / 2;
                        Owner.Character.MagicAttack -= item.Info.MagicAttack / 2;
                    }
                    else
                    {
                        Owner.Character.MinAttack -= item.Info.MinAttack;
                        Owner.Character.MaxAttack -= item.Info.MaxAttack;
                        Owner.Character.MagicAttack -= item.Info.MagicAttack;
                    }
                    Owner.Character.MDefenseByPct -= item.Info.MagicDefense;
                    Owner.Character.PDefense -= item.Info.Defense;
                    Owner.Character.Dodge -= item.Info.Dodge;
                    Owner.Character.BonusHPByItems -= item.Info.VitGive;
                    Owner.Character.BonusHPByItems -= item.Enchant;
                    if (item.Plus > 0)
                    {
                        if (item.Position == Enums.ItemPosition.LeftHand)
                        {
                            Owner.Character.MinAttack -= item.PlusInfo.MinAtk / 2;
                            Owner.Character.MaxAttack -= item.PlusInfo.MaxAtk / 2;
                            Owner.Character.MagicAttack -= item.PlusInfo.MAtk / 2;
                        }
                        else
                        {
                            Owner.Character.MinAttack -= item.PlusInfo.MinAtk;
                            Owner.Character.MaxAttack -= item.PlusInfo.MaxAtk;
                            Owner.Character.MagicAttack -= item.PlusInfo.MAtk;
                        }
                        Owner.Character.MDefenseByVal -= item.PlusInfo.MDef;
                        Owner.Character.PDefense -= item.PlusInfo.Defense;
                        Owner.Character.Dodge -= item.PlusInfo.Dodge;
                        Owner.Character.BonusHPByItems -= item.PlusInfo.HP;
                    }
                }
                #endregion
                equipArray[item.Position] = null;
                Owner.Send(Packets.ToSend.ItemUsage(item.UniqueID, (uint)Enums.ItemUsage.UnequipItem, item.Position, 0));
                Owner.Character.Inventory.TryAdd(item);
                Owner.Character.Screen.Send(Owner.Character.ToBytes(), false);
            }
            return true;
        }
        public Items.Item this[byte ItemPosition]
        {
            get
            {
                Items.Item itm;
                lock (syncroot)
                    itm = equipArray[ItemPosition];
                return itm;
            }
            set
            {
                Equip(value, value.Position);
            }
        }
        public void GetItemList(ref System.Collections.Generic.List<uint> ItemList)
        {
            lock (syncroot)
            {
                for (int i = 0; i < equipArray.Length; i++)
                    if (equipArray[i] != null)
                        ItemList.Add(equipArray[i].UniqueID);
            }
        }
        public void Clear()
        {
            lock (syncroot)
            {
                for (int i = 0; i < equipArray.Length; i++)
                    equipArray = null;
            }
        }
    }
}
