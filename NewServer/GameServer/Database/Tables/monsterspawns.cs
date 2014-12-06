using System;
using System.IO;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadMonsterSpawns()
        {
            foreach (Mapping.Map M in Kernel.Maps.Values)
            {
                if (Directory.Exists("C:/db/Mobs/" + M.UniqueID))
                {
                    string[] Files = Directory.GetFiles("C:/db/Mobs/" + M.UniqueID);
                    foreach (string FilePath in Files)
                    {
                        uint MobID = uint.Parse(FilePath.Remove(0, FilePath.LastIndexOf('\\') + 1).Replace(".ini", ""));
                        Entities.MonsterType MonsterType;
                        if (Kernel.MonsterTypes.TryGetValue(MobID, out MonsterType))
                        {
                            string[] Data = File.ReadAllLines(FilePath);
                            foreach (string spawnData in Data)
                            {
                                if (spawnData != "")
                                {
                                    string[] line = spawnData.Split(' ');
                                    Mapping.MobSpawn Spawn = new Mapping.MobSpawn(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]));
                                    Spawn.UniqueID = Increments.NextSpawnUID;
                                    byte MobAmount = byte.Parse(line[4]);
                                    for (int x = 0; x < MobAmount; x++)
                                    {
                                        Entities.Monster Mob = new Entities.Monster(MonsterType, Spawn);
                                        int px, py;
                                        Spawn.GetLocation(out px, out py);
                                        Mob.Map = M;
                                        Mob.RespawnDelay = Program.Random.Next(1000, 4000);
                                        Mob.X = Mob.SpawnX = (ushort)px;
                                        Mob.Y = Mob.SpawnY = (ushort)py;
                                        M.Insert(Mob);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}