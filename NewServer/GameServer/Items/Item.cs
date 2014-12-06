using System;
using GameServer.Packets;

namespace GameServer.Items
{
    public class Item
    {
        private Writer writer;
        public ItemType Info;
        public ItemPlusInfo PlusInfo;
        public Item(uint ItemID)
        {
            Kernel.ItemTypes.TryGetValue(ItemID, out Info);
            writer = new Writer(48);
            writer.Fill((ushort)48, 0);
            writer.Fill((ushort)1008, 2);
            writer.Fill(ItemID, 8);
            writer.Fill(Info.MaxDurability, 14);
            Mode = Enums.ItemMode.Default;
        }
        public Item(ItemType Info)
        {
            this.Info = Info;
            writer = new Writer(48);
            writer.Fill((ushort)48, 0);
            writer.Fill((ushort)1008, 2);
            writer.Fill(Info.ID, 8);
            writer.Fill(Info.MaxDurability, 14);
            Mode = Enums.ItemMode.Default;
        }
        public Item(FloorItem Item)
        {
            Kernel.ItemTypes.TryGetValue(Item.ItemID, out Info);
            writer = new Writer(48);
            writer.Fill((ushort)48, 0);
            writer.Fill((ushort)1008, 2);
            writer.Fill(Item.ItemID, 8);
            writer.Fill(Info.MaxDurability, 14);
            Mode = Enums.ItemMode.Default;

            this.Durability = Item.Durability;
            this.SocketOne = Item.SocketOne;
            this.SocketTwo = Item.SocketTwo;
            this.Plus = Item.Plus;
            this.Bless = Item.Bless;
            this.Enchant = Item.Enchant;
        }
        public uint OwnerUID;
        public bool Loaded = false;
        private uint _uniqueID;
        public uint UniqueID
        {
            get { return _uniqueID; }
            set
            {
                _uniqueID = value;
                writer.Fill(_uniqueID, 4);
            }
        }

        private ushort _durability;
        public ushort Durability
        {
            get { return _durability; }
            set
            {
                _durability = value;
                writer.Fill(_durability, 12);
                if (Loaded && (_durability % 100 == 0))
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `Durability`=" + _durability + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        private ushort _mode;
        public ushort Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                writer.Fill(_mode, 16);
            }
        }

        private byte _position;
        public byte Position
        {
            get { return _position; }
            set
            {
                _position = value;
                writer.Fill(_position, 18);
                if (Loaded)
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `Position`=" + _position + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        private byte _socketone;
        public byte SocketOne
        {
            get { return _socketone; }
            set
            {
                _socketone = value;
                writer.Fill(_socketone, 24);
                if (Loaded)
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `SocketOne`=" + _socketone + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        private byte _sockettwo;
        public byte SocketTwo
        {
            get { return _sockettwo; }
            set
            {
                _sockettwo = value;
                writer.Fill(_sockettwo, 25);
                if (Loaded)
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `SocketTwo`=" + _sockettwo + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        private byte _plus;
        public byte Plus
        {
            get { return _plus; }
            set
            {
                _plus = value;
                if (_plus > 0)
                    if (Kernel.ItemPlusInfos.ContainsKey(Info.ID))
                        Kernel.ItemPlusInfos[Info.ID].TryGetValue(_plus, out PlusInfo);
                    else
                        _plus = 0;
                writer.Fill(_plus, 28);
                if (Loaded)
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `plus`=" + _plus + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        private byte _bless;
        public byte Bless
        {
            get { return _bless; }
            set
            {
                _bless = value;
                writer.Fill(_bless, 29);
                if (Loaded)
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `Bless`=" + _bless + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        private byte _enchant;
        public byte Enchant
        {
            get { return _enchant; }
            set
            {
                _enchant = value;
                writer.Fill(_enchant, 30);
                if (Loaded)
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `Enchant`=" + _enchant + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        private byte _locked;
        public byte Locked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                writer.Fill(_locked, 38);
                if (Loaded)
                    Database.Database.ItemDB.ExecuteNonQuery("UPDATE `items` SET `Locked`=" + _locked + " WHERE `UniqueID`=" + UniqueID);
            }
        }

        public void QualityChange(Enums.ItemQuality NewQuality)
        {
            uint ID = Info.ID - (Info.ID % 10);
            ID += (byte)NewQuality;
            if (Kernel.ItemTypes.TryGetValue(ID, out Info))
            {
                writer.Fill(Info.ID, 8);
                if (_plus > 0)
                {
                    if (Kernel.ItemPlusInfos.ContainsKey(Info.ID))
                        Kernel.ItemPlusInfos[Info.ID].TryGetValue(_plus, out PlusInfo);
                    else
                        _plus = 0;
                }
            }
        }
        public void ChangeColor(Enums.ItemColor Color)
        {
            uint Tmp = (Info.ID % 100);
            uint ID = (uint)(Info.ID - (Info.ID % 1000) + ((byte)Color * 100) + Tmp);
            if (Kernel.ItemTypes.TryGetValue(ID, out Info))
                writer.Fill(Info.ID, 8);
        }

        public void Save()
        {
            if (!Database.Methods.ContainsItem(UniqueID))
                UniqueID = Database.Methods.InsertItem(this);
        }

        public byte[] ToBytes { get { return writer.Bytes; } }
    }
}
