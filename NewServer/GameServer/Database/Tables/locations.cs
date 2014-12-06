using System;
using System.Data;
using GameServer.Network;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void SaveLocation(uint UniqueID, uint MapID, ushort X, ushort Y)
        {
            Database.CharacterDB.ExecuteNonQuery("UPDATE `locations` SET `MapID`=" + MapID + ", `X`=" + X + ", `Y`=" + Y + " WHERE `UniqueID`=" + UniqueID);
        }
    }
}