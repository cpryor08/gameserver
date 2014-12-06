using System;
namespace GameServer.Items
{
    public struct ItemPlusInfo
    {
        public uint ID;
        public byte Plus;
        public ushort HP;
        public int MinAtk;
        public int MaxAtk;
        public ushort Defense;
        public int MAtk;
        public ushort MDef;
        public ushort Dex;
        public byte Dodge;
    }
}
