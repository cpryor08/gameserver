using System;
using GameServer.Network;
using System.Collections.Concurrent;

namespace GameServer.Packets.ToReceive
{
    public class ItemPacket : Packet
    {
        public int MinLength { get { return 12; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            byte DataType = Data[12];
            switch(DataType)
            {
                case 4:
                    {
                        uint ItemUID = BitConverter.ToUInt32(Data, 4);
                        byte ToPosition = Data[8];
                        Items.Item itm;
                        if (!Client.Character.Inventory.Items.TryGetValue(ItemUID, out itm))
                            return;
                        Client.Character.Inventory.Remove(itm.UniqueID);
                        Client.Character.Equipment.Equip(itm, ToPosition);
                        break;
                    }
                case 6:
                    {
                        if (Client.Character.Dead)
                            return;
                        Client.Character.Equipment.Uneqip(BitConverter.ToUInt32(Data, 4));
                        break;
                    }
                case 9:
                    {
                        ConcurrentBag<Items.Item> Items;
                        if (Client.Character.Warehouses.TryGetValue(Client.Character.MapID, out Items))
                            Client.Send(Packets.ToSend.ViewWarehouse(Client.Character.CurrentDialog.ActiveNPC, Items));
                        else
                            Client.Send(Packets.ToSend.ViewWarehouse(Client.Character.CurrentDialog.ActiveNPC));
                        break;
                    }
                case 10:
                    {
                        uint Amount = BitConverter.ToUInt32(Data, 8);
                        if (Client.Character.Silver >= Amount)
                        {
                            if ((Client.Character.StoredSilver + Amount) < Constants.MaxMoney)
                            {
                                Client.Character.Silver -= Amount;
                                Client.Character.StoredSilver += Amount;
                            }
                            else
                                Client.Send(Packets.ToSend.MessagePacket("You have reached the max amount you can deposit.", "SYSTEM", Client.Character.Name, Enums.ChatType.Top));
                        }
                        else
                            Client.Send(Packets.ToSend.MessagePacket("You don't enough silver for that transaction.", "SYSTEM", Client.Character.Name, Enums.ChatType.Top));
                        break;
                    }
                case 11:
                    {
                        uint Amount = BitConverter.ToUInt32(Data, 8);
                        if (Client.Character.StoredSilver >= Amount)
                        {
                            if (Client.Character.Silver + Amount < Constants.MaxMoney)
                            {
                                Client.Character.StoredSilver -= Amount;
                                Client.Character.Silver += Amount;
                            }
                            else
                                Client.Send(Packets.ToSend.MessagePacket("You have reached the max amount you can carry.", "SYSTEM", Client.Character.Name, Enums.ChatType.Top));
                        }
                        else
                            Client.Send(Packets.ToSend.MessagePacket("You don't enough silver for that transaction.", "SYSTEM", Client.Character.Name, Enums.ChatType.Top));
                        break;
                    }
                case 27: Client.Send(Data); break;
                default: break;
            }
        }
    }
}