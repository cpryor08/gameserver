using System;
namespace AuthServer.Enums
{
    public enum AccountType : byte
    {
        BannedAccount = 0,
        NewAccount = 1,
        RegularUser = 2,
        PlvlAccount = 3,
        PlayerHelper = 4,
        GameManager = 5,
        ProjectManager = 6
    }
}
