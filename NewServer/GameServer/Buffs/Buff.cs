using System;

namespace GameServer.Buffs
{
    public class Buff
    {
        public Enums.SkillExtraEffect ExtraEffect;
        public float Value;
        public uint Transform;
        public int Started;
        public void Start() { NextExpiration = Environment.TickCount + (Duration * 1000); }
        private long _nextexpiration;
        public long NextExpiration
        { get { return _nextexpiration; } set { _nextexpiration = value; } }
        private long _duration;
        public long Duration
        { get { return _duration; } set { _duration = value; } }
        private bool _expires = true;
        public bool Expires
        { get { return _expires; } set { _expires = value; } }
        public void Finished(Entities.Character Owner)
        {
            Buffs.Buff _b;
            Owner.Buffs.Buffs.TryRemove(ExtraEffect, out _b);
        }
    }
}
