using System;
namespace GameServer.Enums
{
    public enum ChatType : ushort
    {
        Talk = 0x7d0,
        Whisper = 0x7d1,
        Action = 0x7d2,
        Team = 0x7d3,
        Guild = 0x7d4,
        Top = 0x7d5,
        Spouse = 0x7d6,
        Yell = 0x7d8,
        Friend = 0x7d9,
        Broadcast = 0x7da,
        Center = 0x7db,
        Ghost = 0x7dd,
        Service = 0x7de,
        Dialog = 0x834,
        LoginInformation = 0x835,
        VendorHawk = 0x838,
        Website = 0x839,
        GWScore1 = 0x83c,
        GWScore2 = 0x83d,
        FriendsOfflineMessage = 0x83e,
        GuildBulletin = 0x83f,
        TradeBoard = 0x899,
        FriendBoard = 0x89a,
        TeamBoard = 0x89b,
        GuildBoard = 0x89c,
        OthersBoard = 0x89d
    }
}
