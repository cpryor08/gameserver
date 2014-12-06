using System;
namespace GameServer.Entities
{
    public struct MonsterType
    {
        public uint ID;
        public bool IsGuard;
        public string Name;
        public byte Level;
        public byte Size;
        public int MinAttack;
        public int MaxAttack;
        public ushort Defense;
        public ushort MagicDefense;
        public uint Model;
        public ushort MaxHealth;
        public byte ViewRange;
        public byte AttackRange;
        public byte Dodge;
    }
}
