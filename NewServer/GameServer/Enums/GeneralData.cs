using System;
namespace GameServer.Enums
{
    public enum GeneralData : ushort
    {
        SetLocation = 74,
        Hotkeys = 75,
        ConfirmFriends = 76,
        ConfirmProfincies = 77,
        ConfirmSpells = 78,
        ChangeDirection = 79,
        ChangeAction = 81,
        ChgMap = 86,
        LevelUp = 92,
        EndXpList = 93,
        Revive = 94,
        ChangePkMode = 96,
        ConfirmGuild = 97,
        BEGIN_MINE = 99,
        TeamLeaderCoords = 101,
        EntitySpawn = 102,
        CompleteMapChange = 104,
        QueryTeamMember = 106,
        RemoveProf = 108, //And Map Pullback, (may be 110 for profs)
        ForgetSpell = 109,
        Shop = 111,
        OpenShop = 113,
        GetSurroundings = 114,
        RemoteCommands = 116,
        PickupCashEffect = 121,
        Dialog = 126,
        CompleteLogin = 130,
        RemoveEntity = 132,
        Jump = 133,
        RemoveWeaponMesh = 135,
        RemoveWeaponMesh2 = 136,
        GuardJump = 137,
        Avatar = 132,
        MapShow3 = 232
    }
}
