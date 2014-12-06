using System;
namespace GameServer.Enums
{
    public enum DamageType : byte
    {
        Magic = 0,
        Ranged = 1,
        Melee = 2,
        HealHP = 3,
        HealMP = 4,
        Percent = 5
    }
}
