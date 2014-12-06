using System;
using System.Data;
using GameServer.Network;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void SaveMoney(uint UniqueID, uint Silver, uint StoredSilver, uint CPs, uint VirtuePoints)
        {
            Database.CharacterDB.ExecuteNonQuery("UPDATE `money` SET `Silver`=" + Silver + ", `StoredSilver`=" + StoredSilver + ", `CPs`=" + CPs + ", `VirtuePoints`=" + VirtuePoints + " WHERE `UniqueID`=" + UniqueID);
        }
    }
}