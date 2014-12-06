using System;
using System.Collections.Concurrent;

namespace GameServer
{
    public class Kernel
    {
        public static ConcurrentDictionary<uint, Mapping.Map> Maps = new ConcurrentDictionary<uint, Mapping.Map>();
        public static ConcurrentDictionary<uint, Entities.MonsterType> MonsterTypes = new ConcurrentDictionary<uint, Entities.MonsterType>();
        public static ConcurrentDictionary<uint, Items.ItemType> ItemTypes = new ConcurrentDictionary<uint, Items.ItemType>();
        public static ConcurrentDictionary<uint, ConcurrentDictionary<byte, Items.ItemPlusInfo>> ItemPlusInfos = new ConcurrentDictionary<uint, ConcurrentDictionary<byte, Items.ItemPlusInfo>>();
        public static ConcurrentDictionary<uint, NPCs.NpcShop> NpcShops = new ConcurrentDictionary<uint, NPCs.NpcShop>();
        public static ConcurrentDictionary<byte, Interface.Event> Events = new ConcurrentDictionary<byte, Interface.Event>();
        public static ConcurrentDictionary<ushort, ConcurrentDictionary<byte, Skills.SkillInfo>> SkillInfos = new ConcurrentDictionary<ushort, ConcurrentDictionary<byte, Skills.SkillInfo>>();
        public static ConcurrentDictionary<uint, byte> WarehouseKeys = new ConcurrentDictionary<uint, byte>();
        public static ConcurrentDictionary<uint, Guilds.Guild> Guilds = new ConcurrentDictionary<uint, Guilds.Guild>();
        public static ConcurrentDictionary<uint, Team> Teams = new ConcurrentDictionary<uint, Team>();
    }
}
