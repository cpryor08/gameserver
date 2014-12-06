using System;
namespace GameServer.Enums
{
    public enum ItemQuality : byte
    {
        Fixed = 0,
        NoUpgrade = 1,
        Simple = 3,
        Poor = 4,
        Normal = 5,
        Refined = 6,
        Unique = 7,
        Elite = 8,
        Super = 9
    }
}
