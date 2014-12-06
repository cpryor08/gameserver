using System;
using GameServer.Network;
using System.Collections.Concurrent;

namespace GameServer.Mapping
{
    public class Screen
    {
        public ConcurrentDictionary<uint, SocketClient> Players = new ConcurrentDictionary<uint, SocketClient>();
        public ConcurrentDictionary<uint, Entities.Monster> Monsters = new ConcurrentDictionary<uint, Entities.Monster>();
        public ConcurrentDictionary<uint, Entities.NPC> NPCs = new ConcurrentDictionary<uint, Entities.NPC>();
        public ConcurrentDictionary<uint, Items.FloorItem> FloorItems = new ConcurrentDictionary<uint, Items.FloorItem>();
        public SocketClient Owner;
        public Screen(SocketClient Owner)
        {
            this.Owner = Owner;
        }
        public bool Insert(SocketClient Client)
        {
            if (Client.UniqueID == Owner.UniqueID)
                return false;
            if (Players.TryAdd(Client.UniqueID, Client))
            {
                Owner.Send(Client.Character.ToBytes());
                return true;
            }
            return false;
        }
        public bool Insert(Entities.Monster Monster)
        {
            if (Monsters.TryAdd(Monster.UniqueID, Monster))
            {
                Monster.Screen.TryAdd(Owner.UniqueID, Owner);
                Owner.Send(Monster.ToBytes());
                return true;
            }
            if (Monster.Target == null)
                Monster.Target = Owner;
            else if (Calculations.GetDistance(Monster.X, Monster.Y, Monster.Target.Character.X, Monster.Target.Character.Y) > Calculations.GetDistance(Monster.X, Monster.Y, Owner.Character.X, Owner.Character.Y))
                Monster.Target = Owner;
            return false;
        }
        public bool Insert(Entities.NPC Npc)
        {
            if (NPCs.TryAdd(Npc.UniqueID, Npc))
            {
                Owner.Send(Npc.ToBytes);
                return true;
            }
            return false;
        }
        public bool Insert(Items.FloorItem Item)
        {
            if (FloorItems.TryAdd(Item.UniqueID, Item))
            {
                Owner.Send(Packets.ToSend.ItemDrop(Item, Item.X, Item.Y, false));
                return true;
            }
            return false;
        }
        public bool Remove(uint UniqueID)
        {
            if (UniqueID < Constants.Min_MobUID)
            {
                Entities.NPC removedNpc;
                if (NPCs.TryRemove(UniqueID, out removedNpc))
                {
                    Owner.Send(Packets.ToSend.GeneralData(removedNpc.UniqueID, 0, 0, 0, 0, Enums.GeneralData.RemoveEntity));
                    return true;
                }
            }
            else if (UniqueID <= Constants.Max_MobUID)
            {
                Entities.Monster removedMonster;
                if (Monsters.TryRemove(UniqueID, out removedMonster))
                {
                    if (removedMonster.Target != null)
                        if (removedMonster.Target.UniqueID == Owner.UniqueID)
                            removedMonster.Target = null;
                    SocketClient removedClient;
                    removedMonster.Screen.TryRemove(Owner.UniqueID, out removedClient);
                    Owner.Send(Packets.ToSend.GeneralData(removedMonster.UniqueID, 0, 0, 0, 0, Enums.GeneralData.RemoveEntity));
                    return true;
                }
            }
            else
            {
                SocketClient removedClient;
                if (Players.TryRemove(UniqueID, out removedClient))
                {
                    Owner.Send(Packets.ToSend.GeneralData(removedClient.UniqueID, 0, 0, 0, 0, Enums.GeneralData.RemoveEntity));
                    return true;
                }
            }
            return false;
        }
        public bool Contains(uint UniqueID)
        {
            if (UniqueID < Constants.Max_MobUID)
                return Monsters.ContainsKey(UniqueID);
            else
                return Players.ContainsKey(UniqueID);
        }
        public void Clear()
        {
            foreach (SocketClient sC in Players.Values)
            {
                Remove(sC.UniqueID);
                sC.Character.Screen.Remove(Owner.UniqueID);
            }
            foreach (Entities.Monster mob in Monsters.Values)
                Remove(mob.UniqueID);
        }
        public void Clean()
        {
            foreach (SocketClient sC in Players.Values)
                if (Calculations.GetDistance(sC.Character.X, sC.Character.Y, Owner.Character.X, Owner.Character.Y) >= Constants.ScreenDistance)
                    Remove(sC.UniqueID);
            foreach (Entities.Monster Monster in Monsters.Values)
                if (Calculations.GetDistance(Monster.X, Monster.Y, Owner.Character.X, Owner.Character.Y) >= Constants.ScreenDistance)
                    Remove(Monster.UniqueID);
        }
        public void Send(byte[] Data, bool SendSelf)
        {
            foreach (SocketClient sC in Players.Values)
                sC.Send(Data);
            if (SendSelf)
                Owner.Send(Data);
        }
    }
}