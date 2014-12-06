using System;
using System.Text;
using System.Collections.Concurrent;
namespace GameServer.Guilds
{
    public class Guild
    {
        public uint GuildID;
        private ConcurrentDictionary<uint, string> MemberStrings = new ConcurrentDictionary<uint, string>();
        public void AddMemString(uint UniqueID, string MemStr)
        {
            MemberStrings.TryAdd(UniqueID, MemStr);
        }
        public void UpdateMemberString(uint UniqueID, string Name, byte Level, bool Online)
        {
            if (MemberStrings.ContainsKey(UniqueID))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Name);
                sb.Append(" ");
                sb.Append(Level);
                sb.Append(" ");
                sb.Append(Online ? 1 : 0);
                MemberStrings[UniqueID] = sb.ToString();
            }
        }
    }
}
