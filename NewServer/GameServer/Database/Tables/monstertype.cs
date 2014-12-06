using System;
using System.IO;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadMonsterTypes()
        {
            if (File.Exists("C:/db/MonsterType.dat"))
            {
                string[] MonsterTypes = File.ReadAllLines("C:/db/MonsterType.dat");
                for (int i = 0; i < MonsterTypes.Length; i++)
                {
                    string[] MonsterInfo = MonsterTypes[i].Split(' ');
                    Entities.MonsterType MT = new Entities.MonsterType()
                    {
                        ID = uint.Parse(MonsterInfo[0]),
                        Name = MonsterInfo[1],
                        //type = 2
                        Model = ushort.Parse(MonsterInfo[3]),
                        MaxHealth = ushort.Parse(MonsterInfo[4]),
                        //mana 5
                        MaxAttack = int.Parse(MonsterInfo[6]),
                        MinAttack = int.Parse(MonsterInfo[7]),
                        Defense = ushort.Parse(MonsterInfo[8]),
                        //dex = 9
                        Dodge = byte.Parse(MonsterInfo[10]),
                        AttackRange = 2, //byte.Parse(MonsterInfo[14])
                        ViewRange = byte.Parse(MonsterInfo[15]),
                        //escape_life 16
                        //attack_speed 17
                        //move_speed 18
                        Level = byte.Parse(MonsterInfo[19]),
                        //attack_user 20
                        //drop_money 21
                        //drop_itemtype 22
                        Size = byte.Parse(MonsterInfo[23]),
                        //run_speed 24
                        //drop_armet 25
                        //drop_necklace 26
                        //drop_armor 27
                        //drop_ring 28
                        //drop_weapon 29
                        //drop_shield 30
                        //drop_shoes 31
                        //drop_hp 32
                        //drop_mp 33
                        //magic_type 34
                        //MagicDefense = ushort.Parse(MonsterInfo[35])
                        //magic_hitrate 36
                        //ai_type 37
                        //defense2 38
                        //extra_exp 39
                        //extra_damage 40
                    };
                    if (MT.ID == 900 || MT.ID == 910)
                        MT.IsGuard = true;
                    Kernel.MonsterTypes.TryAdd(MT.ID, MT);
                }
            }
            else
            {
                Console.WriteLine("Failed to load Monster Types. File Not Found.");
                Console.ReadKey();
            }
        }
    }
}