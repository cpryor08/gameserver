using System;
namespace GameServer.Enums
{
    public enum AttackType : ushort
    {
        None = 0x00,
        Physical = 0x02,
        Magic = 0x15,
        Archer = 0x19,
        RequestMarriage = 0x08,
        AcceptMarriage = 0x09,
        Death = 0x0E
    }
}
