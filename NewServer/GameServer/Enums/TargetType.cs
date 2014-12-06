using System;
namespace GameServer.Enums
{
    public enum TargetType : byte
    {
        Single,
        FromSingle,
        FromPoint,
        Range,
        Sector,
        Linear
    }
}
