using System;
using System.Data;
using GameServer.Network;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadProfs(SocketClient Client)
        {
            using (DataTable DT = Database.CharacterDB.GetDataTable("SELECT `ID`, `Level`, `Experience` FROM `profs` WHERE `OwnerUID`=" + Client.UniqueID))
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DataRow dr = DT.Rows[i];
                    Profs.Prof P = new Profs.Prof();
                    ushort ProfID = Convert.ToUInt16(dr.ItemArray[0]);
                    P.Level = Convert.ToByte(dr.ItemArray[1]);
                    P.Experience = Convert.ToUInt32(dr.ItemArray[2]);
                    if (Client.Character.Profs.TryAdd(ProfID, P))
                        Client.Send(Packets.ToSend.ProfPacket(ProfID, P));
                }
            }
        }
    }
}
