using System;
using System.Collections.Concurrent;

namespace GameServer.Buffs
{
    public sealed class BuffCollection
    {
        public ConcurrentDictionary<Enums.SkillExtraEffect, Buffs.Buff> Buffs = new ConcurrentDictionary<Enums.SkillExtraEffect, Buffs.Buff>();
        public long Flags;

        public Boolean ContainsFlag(Enums.Flag Flag) { return (Flags & (long)Flag) != 0; }
        public void AddFlag(Enums.Flag Flag) { Flags |= (long)Flag; }
        public void DelFlag(Enums.Flag Flag) { Flags &= ~(long)Flag; }

        public void Clear()
        {
            this.Buffs.Clear();
        }
    }
}
