using System;
namespace GameServer.Enums
{
    public enum TeamPacket : byte
    {
        Create = 0,
        JoinRequest = 1,
        ExitTeam = 2,
        AcceptInvitation = 3,
        InviteRequest = 4,
        AcceptJoinRequest = 5,
        Dismiss = 6,
        Kick = 7,
        ForbidJoining = 8,
        UnforbidJoining = 9,
        LootMoneyOff = 10,
        LootMoneyOn = 11,
        LootItemsOff = 12,
        LootItemsOn = 13
    }
}
