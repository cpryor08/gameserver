namespace GameServer.Skills
{
    public struct SkillInfo
    {
        public ushort ID;
        public byte Level;
        public uint Experience;
        public ushort ManaCost;
        public byte StaminaCost;
        public byte ArrowCost;
        public bool EndsXPWait;
        public byte UpgReqLev;
        public uint UpgReqExp;
        public uint Damage;
        public Enums.TargetType TargetType;
        public Enums.DamageType DamageType;
        public Enums.SkillExtraEffect ExtraEffect;
        public ushort EffectLasts;
        public float EffectValue;
        public byte ActivationChance;
        public byte Range;
        public byte SectorSize;
        public bool CanTargetSelf;
        public bool AutoAttack;
        public ushort Delay;
    }
}