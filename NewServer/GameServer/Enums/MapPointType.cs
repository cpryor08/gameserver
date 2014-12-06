using System;
namespace GameServer.Enums
{
    public enum MapPointType : byte
    {
        Empty = 1,
        ContainsMonster = 2,
        ContainsItem = 3,
        ContainsMonsterAndItem = 4,
        Invalid = 255
    }
}