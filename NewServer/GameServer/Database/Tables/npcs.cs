using System;
using System.Data;
using GameServer.Packets;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadNPCs()
        {
            Database db = new Database("C:/db/npcs.s3db");
            using (DataTable DT = db.GetDataTable("SELECT `UniqueID`, `Name`, `Type`, `Facing`, `MapID`, `X`, `Y` FROM `npcs`"))
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DataRow dr = DT.Rows[i];
                    Entities.NPC npc = new Entities.NPC();
                    npc.UniqueID = Convert.ToUInt32(dr.ItemArray[0]);
                    npc.Name = dr.ItemArray[1].ToString();
                    npc.Type = Convert.ToUInt16(dr.ItemArray[2]);
                    npc.Facing = (Enums.ConquerAngle)Convert.ToByte(dr.ItemArray[3]);
                    npc.X = Convert.ToUInt16(dr.ItemArray[5]);
                    npc.Y = Convert.ToUInt16(dr.ItemArray[6]);

                    int Len = 20;
                    if (npc.Name != "Unknown") { Len += npc.Name.Length; }
                    Writer PWR = new Writer(Len);
                    PWR.Fill((ushort)Len, 0);
                    PWR.Fill((ushort)2030, 2);
                    PWR.Fill((uint)npc.UniqueID, 4);
                    PWR.Fill((ushort)npc.X, 8);
                    PWR.Fill((ushort)npc.Y, 10);
                    PWR.Fill((ushort)npc.Type, 12);
                    PWR.Fill((byte)npc.Facing, 14);
                    PWR.Fill((ushort)0, 16);
                    if (npc.Name != "Unknown")
                    {
                        PWR.Fill((byte)1, 18);
                        PWR.Fill((byte)npc.Name.Length, 19);
                        PWR.Fill(npc.Name, 20);
                    }
                    npc.ToBytes = PWR.Bytes;
                    Mapping.Map m;
                    if (Kernel.Maps.TryGetValue(Convert.ToUInt32(dr.ItemArray[4]), out m))
                        m.Insert(npc);
                }
            }
            db.Dispose();
        }
    }
}