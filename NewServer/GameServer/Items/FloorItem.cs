using System;
namespace GameServer.Items
{
    public class FloorItem
    {
        public byte SocketOne;
        public byte SocketTwo;
        public byte Plus;
        public byte Bless;
        public byte Enchant;
        public ushort Durability;
        public ushort X;
        public ushort Y;
        public uint ItemID;
        public uint UniqueID;
        public uint OwnerUID;
        public int DropTime = 0;
    }
}