using System;
using System.Data;
namespace GameServer.Database
{
    public partial class Methods
    {
        public static bool Authenticated(uint UniqueID)
        {
            bool toReturn = false;
            using (DataTable DT = Database.AccountDB.GetDataTable("SELECT `Date` FROM `login_tokens` WHERE `PlayerUID`=" + UniqueID + " ORDER BY `Date` DESC LIMIT 1"))
            {
                if (DT.Rows.Count == 1)
                {
                    toReturn = ((DateTime.Now - Convert.ToDateTime(DT.Rows[0].ItemArray[0])).TotalSeconds < 30);
                    Database.AccountDB.ExecuteNonQuery("DELETE FROM `login_tokens` WHERE `PlayerUID`=" + UniqueID);
                }
            }
            return toReturn;
        }
    }
}