using System;
using System.Text;
using GameServer.Network;

namespace GameServer.Packets.ToReceive
{
    public class ChatPacket : Packet
    {
        public int MinLength { get { return 29; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            Enums.ChatType ChatType = (Enums.ChatType)BitConverter.ToUInt32(Data, 8);
            string From = Encoding.ASCII.GetString(Data, 26, Data[25]);
            if (From != Client.Character.Name)
                return;
            string To = Encoding.ASCII.GetString(Data, 27 + From.Length, Data[26 + From.Length]);
            string Message = Encoding.ASCII.GetString(Data, 29 + From.Length + To.Length, Data[28 + From.Length + To.Length]);
            if (Message.StartsWith(Constants.CommandOperator))
            {
                Message = Message.Replace(Constants.CommandOperator, "");
                string[] msgData = Message.Split(' ');
                switch (msgData[0])
                {
                    case "life": Client.Character.HitPoints = Client.Character.MaxHitPoints; break;
                    case "dc": Client.Disconnect(); break;
                    case "money":
                    case "silver":
                        {
                            if (msgData.Length < 2)
                                return;
                            uint Silver;
                            if (!uint.TryParse(msgData[1], out Silver))
                                return;
                            Client.Character.Silver = Silver;
                            break;
                        }
                    case "learnspell":
                        {
                            if (msgData.Length < 3)
                                return;
                            ushort SkillID;
                            if (!ushort.TryParse(msgData[1], out SkillID))
                                return;
                            byte Level;
                            if (!byte.TryParse(msgData[2], out Level))
                                return;
                            Client.Character.LearnSkill(SkillID, Level, 0);
                            break;
                        }
                    case "forgetspell":
                        {
                            if (msgData.Length < 2)
                                return;
                            ushort SkillID;
                            if (!ushort.TryParse(msgData[1], out SkillID))
                                return;
                            Client.Character.ForgetSkill(SkillID);
                            break;
                        }
                    case "item":
                        {
                            if (msgData.Length > 2)
                            {
                                uint ItemID;
                                if (!uint.TryParse(msgData[1], out ItemID))
                                {
                                    if (msgData.Length == 8)
                                    {
                                        Enums.ItemQuality quality;
                                        switch (msgData[2].ToLower())
                                        {
                                            case "super": quality = Enums.ItemQuality.Super; break;
                                            case "elite": quality = Enums.ItemQuality.Elite; break;
                                            case "unique": quality = Enums.ItemQuality.Unique; break;
                                            case "refined": quality = Enums.ItemQuality.Refined; break;
                                            case "normal": quality = Enums.ItemQuality.Normal; break;
                                            default: return;
                                        }
                                        byte Plus;
                                        if (!byte.TryParse(msgData[3], out Plus))
                                            return;
                                        byte Bless;
                                        if (!byte.TryParse(msgData[4], out Bless))
                                            return;
                                        byte Enchant;
                                        if (!byte.TryParse(msgData[5], out Enchant))
                                            return;
                                        byte Gem1;
                                        if (!byte.TryParse(msgData[6], out Gem1))
                                            return;
                                        byte Gem2;
                                        if (!byte.TryParse(msgData[7], out Gem2))
                                            return;
                                        foreach (Items.ItemType ItemInfo in Kernel.ItemTypes.Values)
                                        {
                                            if (ItemInfo.Name == msgData[1])
                                            {
                                                Items.Item Itm = new Items.Item(ItemInfo);
                                                Itm.OwnerUID = Client.UniqueID;
                                                Itm.Bless = Bless;
                                                Itm.Enchant = Enchant;
                                                Itm.QualityChange(quality);
                                                Itm.Durability = ItemInfo.MaxDurability;
                                                Itm.Mode = 1;
                                                Itm.Plus = Plus;
                                                if (ItemInfo.ID >= 730001 && ItemInfo.ID <= 730009)
                                                    Itm.Plus = (byte)(ItemInfo.ID - 730000);
                                                Itm.Position = (byte)Enums.ItemPosition.Inventory;
                                                Itm.Loaded = true;
                                                Itm.Save();
                                                Client.Character.Inventory.TryAdd(Itm);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    default: break;
                }
            }
            else
            {
                uint Color = BitConverter.ToUInt32(Data, 4);
                switch(ChatType)
                {
                    case Enums.ChatType.Talk:
                        {
                            Client.Character.Screen.Send(Packets.ToSend.MessagePacket(Message, From, To, Client.Character.Dead ? Enums.ChatType.Ghost : Enums.ChatType.Talk), false);
                            break;
                        }
                    default: break;
                }
            }
        }
    }
}