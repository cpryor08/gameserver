using System;
namespace GameServer.Enums
{
    public enum Flag : long
    {
        None = 0x00,
        Flashing = 0x01,
        Poisoned = 0x02,
        //Invisible = 0x04,
        //Unknow = 0x08
        XPList = 0x10,
        Frozen = 0x20,
        TeamLeader = 0x40,
        Accuracy = 0x80,
        Shield = 0x100,
        Stigma = 0x200,
        Die = 0x400,
        //FadeOut = 0x800,
        AzureShield = 0x1000,
        //0x2000 -> None
        RedName = 0x4000,
        BlackName = 0x8000,
        //0x10000 -> None
        //0x20000 -> None
        SuperMan = 0x40000,
        ThirdMetempsychosis = 0x80000,
        FourthMetempsychosis = 0x100000,
        FifthMetempsychosis = 0x200000,
        Invisibility = 0x400000,
        Cyclone = 0x800000,
        SixthMetempsychosis = 0x1000000,
        SeventhMetempsychosis = 0x2000000,
        EighthMetempsychosis = 0x4000000,
        Flying = 0x8000000,
        NinthMetempsychosis = 0x10000000,
        TenthMetempsychosis = 0x20000000,
        CastingPray = 0x40000000,
        Praying = 0x80000000,
    };
}
