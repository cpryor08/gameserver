using System;
using System.Data;
using GameServer.Network;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static bool NameExists(string Name)
        {
            return Convert.ToUInt32(Database.CharacterDB.GetDataTable("SELECT COUNT(*) FROM `info` WHERE `name`='" + Name + "' LIMIT 1")) > 0;
        }
        public static bool HasCharacter(uint UniqueID)
        {
            return Convert.ToUInt32(Database.CharacterDB.ExecuteScalar("SELECT COUNT(*) FROM `info` WHERE `UniqueID`=" + UniqueID)) > 0;
        }
        public static bool LoadCharacter(SocketClient Client)
        {
            using (DataTable DT = Database.CharacterDB.GetDataTable(@"SELECT i.name, i.class, i.level, i.experience, i.spouse, i.rbcount, i.hitpoints, i.pkpoints, i.mana, a.hairstyle, a.model, a.avatar, l.mapid, l.x, l.y, m.silver, m.storedsilver, m.cps, m.virtuepoints, s.strength, s.agility, s.spirit, s.vitality, s.statpoints FROM `info` as `i` INNER JOIN `appearance` as `a` ON i.UniqueID=a.UniqueID INNER JOIN `locations` as `l` ON i.UniqueID=l.UniqueID INNER JOIN `money` as `m` ON i.UniqueID=m.UniqueID INNER JOIN `stats` as `s` ON i.UniqueID=s.UniqueID WHERE i.UniqueID=" + Client.UniqueID))
            {
                if (DT.Rows.Count == 1)
                {
                    DataRow dr = DT.Rows[0];
                    Client.Character = new Entities.Character(Client.UniqueID, Client);
                    Client.Character.Name = dr.ItemArray[0].ToString();
                    Client.Character.Class = Convert.ToByte(dr.ItemArray[1]);
                    Client.Character.Level = Convert.ToByte(dr.ItemArray[2]);
                    Client.Character.Experience = Convert.ToUInt32(dr.ItemArray[3]);
                    Client.Character.Spouse = Convert.ToUInt32(dr.ItemArray[4]);
                    Client.Character.RBCount = Convert.ToByte(dr.ItemArray[5]);
                    Client.Character.HitPoints = Convert.ToUInt16(dr.ItemArray[6]);
                    Client.Character.PKPoints = Convert.ToUInt16(dr.ItemArray[7]);
                    Client.Character.Mana = Convert.ToUInt16(dr.ItemArray[8]);
                    Client.Character.HairStyle = Convert.ToUInt16(dr.ItemArray[9]);
                    Client.Character.Model = Convert.ToUInt32(dr.ItemArray[10]);
                    Client.Character.Avatar = Convert.ToUInt16(dr.ItemArray[11]);
                    Client.Character.MapID = Convert.ToUInt32(dr.ItemArray[12]);
                    Client.Character.X = Convert.ToUInt16(dr.ItemArray[13]);
                    Client.Character.Y = Convert.ToUInt16(dr.ItemArray[14]);
                    Client.Character.Silver = Convert.ToUInt32(dr.ItemArray[15]);
                    Client.Character.StoredSilver = Convert.ToUInt32(dr.ItemArray[16]);
                    Client.Character.CPs = Convert.ToUInt32(dr.ItemArray[17]);
                    Client.Character.VirtuePoints = Convert.ToUInt32(dr.ItemArray[18]);
                    Client.Character.Strength = Convert.ToUInt16(dr.ItemArray[19]);
                    Client.Character.Agility = Convert.ToUInt16(dr.ItemArray[20]);
                    Client.Character.Spirit = Convert.ToUInt16(dr.ItemArray[21]);
                    Client.Character.Vitality = Convert.ToUInt16(dr.ItemArray[22]);
                    Client.Character.StatPoints = Convert.ToUInt16(dr.ItemArray[23]);
                    return true;
                }
            }
            return false;
        }
        public static void CreateCharacter(uint UniqueID, string Name, ushort Class, ushort Model)
        {
            byte Avatar = 67;
            if (Model == 2001 || Model == 2002)
                Avatar = 201;
            int HairStyle = 0;
            int Rnd = Program.Random.Next(0, 100);
            if (Rnd <= 20)
                HairStyle = Program.Random.Next(9, 17);
            else if (Rnd <= 40)
                HairStyle = Program.Random.Next(34, 42);
            else if (Rnd <= 60)
                HairStyle = Program.Random.Next(21, 25);
            else if (Rnd <= 80)
                HairStyle = Program.Random.Next(30, 33);
            else
                HairStyle = Program.Random.Next(43, 51);
            HairStyle = (Program.Random.Next(3, 9) * 100) + HairStyle;
            Database.CharacterDB.ExecuteNonQuery("INSERT INTO `info` (`UniqueID`, `Name`, `Class`) VALUES ('" + UniqueID + "', '" + Name + "', '" + Class + "')");
            Database.CharacterDB.ExecuteNonQuery("INSERT INTO `appearance` (`UniqueID`, `HairStyle`, `Model`, `Avatar`) VALUES ('" + UniqueID + "', '" + HairStyle + "', '" + Model + "', '" + Avatar + "')");
            Database.CharacterDB.ExecuteNonQuery("INSERT INTO `locations` (`UniqueID`, `MapID`, `X`, `Y`) VALUES ('" + UniqueID + "', '1002', '429', '378')");
            Database.CharacterDB.ExecuteNonQuery("INSERT INTO `money` (`UniqueID`, `Silver`) VALUES ('" + UniqueID + "', '1000')");
            Database.CharacterDB.ExecuteNonQuery("INSERT INTO `stats` (`UniqueID`, `Strength`, `Agility`, `Spirit`, `Vitality`, `StatPoints`) VALUES ('" + UniqueID + "', '0', '0', '0', '0', '100')");
        }
        public static void SaveUserInfo(uint UniqueID, uint Experience, ushort HitPoints, ushort Mana, ushort PKPoints)
        {
            Database.CharacterDB.ExecuteNonQuery("UPDATE `info` SET `Experience`=" + Experience + ", `HitPoints`=" + HitPoints + ", `Mana`=" + Mana + ", `PKPoints`=" + PKPoints + " WHERE `UniqueID`=" + UniqueID);
        }
    }
}
