using System;
namespace GameServer.Enums
{
    public enum SyncType : uint
    {
        Hitpoints = 0x00,
        MaxHitpoints = 0x01,
        Mana = 0x02,
        MaxMana = 0x03,
        Money = 0x04,
        Experience = 0x05,
        PKPoints = 0x06,
        Job = 0x07,
        None = 0xFFFFFFFF,
        qwRaiseFlag = 0x08,
        Stamina = 0x09,
        StatPoints = 0x0B,
        Reborn = 0x22,
        Mesh = 0x0C,
        Level = 0x0D,
        Spirit = 0x0E,
        Vitality = 0x0F,
        Strength = 0x10,
        Agility = 0x11,
        RaiseFlag = 0x1A,
        Hairstyle = 0x1B,
        ConquerPoints = 0x1E,
        XPCircle = 0x1F,
        DoubleExpTimer = 0x13,
        TripleExpTimer = 0x13,
        CursedTimer = 0x15,
        LuckyTimeTimer = 0x1D,
        HeavensBlessing = 0x12
    }
}
