using System;
namespace GameServer.Items
{
    public struct ItemType
    {
        public uint ID;
        public string Name;
        public byte ClassReq;
        public byte ProfReq;
        public byte LvlReq;
        public byte SexReq;
        public ushort StrReq;
        public ushort AgiReq;
        public byte PosReq;
        public uint Cost;
        public ushort MinAttack;
        public ushort MaxAttack;
        public ushort Defense;
        public byte MagicDefense;
        public ushort MagicAttack;
        public byte Dodge;
        public byte AgiGive;
        public ushort VitGive;
        public ushort ManaGive;
        public ushort CPsWorth;
        public ushort MaxDurability;
        public byte Range;
    }
}
