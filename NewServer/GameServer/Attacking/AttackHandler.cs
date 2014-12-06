using System;
using GameServer.Network;
using GameServer.Packets;
using System.Collections.Generic;
namespace GameServer.Attacking
{
    public class AttackHandler
    {
        public bool Attacking;
        public int AttackSpeed;
        public int NextAttack;
        private SocketClient Owner;
        private Writer packetWriter = new Writer();
        private byte[] LastAttack;
        public AttackHandler(SocketClient Owner)
        {
            this.Owner = Owner;
        }
        public void Handle() { this.Handle(LastAttack, true); }
        public void Handle(byte[] Data, bool AutoAttack = false)
        {
            int Time = Environment.TickCount;
            if (NextAttack > Time)
                return;
            if (!AutoAttack)
                this.LastAttack = Data;
            AutoAttack = false;
            NextAttack = Time + AttackSpeed;
            uint AttackerUID = BitConverter.ToUInt32(Data, 8);
            uint AttackedUID = BitConverter.ToUInt32(Data, 12);
            Enums.AttackType AttackType = (Enums.AttackType)BitConverter.ToUInt16(Data, 20);
            switch (AttackType)
            {
                case Enums.AttackType.Physical:
                case Enums.AttackType.Archer: SingleTargetAttack(AttackedUID, AttackType); break;
                case Enums.AttackType.Magic: MagicAttack(Data, AttackedUID); break;

                case Enums.AttackType.RequestMarriage: break;
                case Enums.AttackType.AcceptMarriage: break;

                case Enums.AttackType.Death: break;
                case Enums.AttackType.None: break;
                default: break;
            }
        }
        private void SingleTargetAttack(uint AttackedUID, Enums.AttackType AttackType)
        {
            if (AttackedUID >= Constants.Min_MobUID && AttackedUID <= Constants.Max_MobUID)
            {
                Entities.Monster Target;
                if (!Owner.Character.Screen.Monsters.TryGetValue(AttackedUID, out Target))
                    return;
                if (!Owner.Character.CanTarget(Target))
                    return;
                uint Damage = 0;
                if (AttackType == Enums.AttackType.Archer)
                    Damage = Calculations.Attacks.RangedPlayerVsMonster(Owner.Character, Target);
                else if (AttackType == Enums.AttackType.Physical)
                    Damage = Calculations.Attacks.MeleePlayerVsMonster(Owner.Character, Target);
                else
                    return;
                Target.SendScreen(Packets.ToSend.Attack(Owner.UniqueID, AttackedUID, Target.X, Target.Y, Damage, AttackType));
                Target.TakeDamage(Owner.UniqueID, Damage);
            }
            else if (AttackedUID >= Constants.Min_PlayerUID && AttackedUID <= Constants.Max_PlayerUID)
            {
                Network.SocketClient Target;
                if (!Owner.Character.Screen.Players.TryGetValue(AttackedUID, out Target))
                    return;
                if (!Owner.Character.CanTarget(Target.Character))
                    return;
                uint Damage = 0;
                if (AttackType == Enums.AttackType.Archer)
                    Damage = Calculations.Attacks.RangedPlayerVsPlayer(Owner.Character, Target.Character);
                else if (AttackType == Enums.AttackType.Physical)
                    Damage = Calculations.Attacks.MeleePlayerVsPlayer(Owner.Character, Target.Character);
                else
                    return;
                Target.Character.Screen.Send(Packets.ToSend.Attack(Owner.UniqueID, AttackedUID, Target.Character.X, Target.Character.Y, Damage, AttackType), true);
                Target.Character.TakeDamage(Owner.UniqueID, Damage);
            }
        }
        private void MagicAttack(byte[] Data, uint AttackedUID)
        {
            ushort SkillId = Convert.ToUInt16(((long)Data[24] & 0xFF) | (((long)Data[25] & 0xFF) << 8));
            SkillId ^= (ushort)0x915d;
            SkillId ^= (ushort)Owner.UniqueID;
            SkillId = (ushort)(SkillId << 0x3 | SkillId >> 0xd);
            SkillId -= 0xeb42;

            Skills.Skill Skill;
            if (!Owner.Character.Skills.TryGetValue(SkillId, out Skill))
                return;

            long x = (Data[16] & 0xFF) | ((Data[17] & 0xFF) << 8);
            long y = (Data[18] & 0xFF) | ((Data[19] & 0xFF) << 8);

            x = x ^ (uint)(Owner.UniqueID & 0xffff) ^ 0x2ed6;
            x = ((x << 1) | ((x & 0x8000) >> 15)) & 0xffff;
            x |= 0xffff0000;
            x -= 0xffff22ee;

            y = y ^ (uint)(Owner.UniqueID & 0xffff) ^ 0xb99b;
            y = ((y << 5) | ((y & 0xF800) >> 11)) & 0xffff;
            y |= 0xffff0000;
            y -= 0xffff8922;

            if (Owner.Character.Mana < Skill.SkillInfo.ManaCost)
                return;
            if (Owner.Character.Stamina < Skill.SkillInfo.StaminaCost)
                return;
            if (Skill.SkillInfo.ArrowCost > 0)
            {
#warning finish arrow cost
            }

            switch(Skill.SkillInfo.TargetType)
            {
                #region Single
                case Enums.TargetType.Single:
                    {
                        if (AttackedUID >= Constants.Min_MobUID && AttackedUID <= Constants.Max_MobUID)
                        {
                            Entities.Monster Target;
                            if (!Owner.Character.Screen.Monsters.TryGetValue(AttackedUID, out Target))
                                return;
                            if (!Owner.Character.CanTarget(Target))
                                return;
                            uint Damage = Calculations.Attacks.MagicPlayerVsMonster(Owner.Character, Target, Skill);
                            if (Skill.SkillInfo.DamageType == Enums.DamageType.HealHP)
                            {
                                Target.SendScreen(Packets.ToSend.SingleSpellUse(Owner.UniqueID, Target.UniqueID, Damage, Skill.SkillInfo.ID, Skill.SkillInfo.Level, Target.X, Target.Y));
                                Target.HitPoints = Math.Min(Target.Info.MaxHealth, (ushort)(Target.HitPoints + Damage));
                            }
                            else if (Skill.SkillInfo.DamageType == Enums.DamageType.HealMP)
                                return;
                            else
                            {
                                Target.SendScreen(Packets.ToSend.SingleSpellUse(Owner.UniqueID, Target.UniqueID, Damage, Skill.SkillInfo.ID, Skill.SkillInfo.Level, Target.X, Target.Y));
                                Target.TakeDamage(Owner.UniqueID, Damage);
                            }
                        }
                        else if (AttackedUID >= Constants.Min_PlayerUID && AttackedUID <= Constants.Max_PlayerUID)
                        {
                            Network.SocketClient Target;
                            if (!Owner.Character.Screen.Players.TryGetValue(AttackedUID, out Target))
                                return;
                            if (!Owner.Character.CanTarget(Target.Character, Skill))
                                return;
                            uint Damage = Calculations.Attacks.MagicPlayerVsPlayer(Owner.Character, Target.Character, Skill);
                            if (Skill.SkillInfo.DamageType == Enums.DamageType.HealHP)
                            {
                                Target.Character.Screen.Send(Packets.ToSend.SingleSpellUse(Owner.UniqueID, Target.UniqueID, Damage, Skill.SkillInfo.ID, Skill.SkillInfo.Level, Target.Character.X, Target.Character.Y), true);
                                Target.Character.HitPoints = Math.Min(Target.Character.MaxHitPoints, (ushort)(Target.Character.HitPoints + Damage));
                            }
                            else if (Skill.SkillInfo.DamageType == Enums.DamageType.HealMP)
                            {
                                Target.Character.Screen.Send(Packets.ToSend.SingleSpellUse(Owner.UniqueID, Target.UniqueID, Damage, Skill.SkillInfo.ID, Skill.SkillInfo.Level, Target.Character.X, Target.Character.Y), true);
                                Target.Character.Mana = Math.Min(Target.Character.MaxMana, (ushort)(Target.Character.Mana + Damage));
                            }
                            else
                            {
                                Target.Character.Screen.Send(Packets.ToSend.SingleSpellUse(Owner.UniqueID, Target.UniqueID, Damage, Skill.SkillInfo.ID, Skill.SkillInfo.Level, Target.Character.X, Target.Character.Y), true);
                                Target.Character.TakeDamage(Owner.UniqueID, Damage);
                            }
                        }
                        break;
                    }
                #endregion
                #region Range
                case Enums.TargetType.Range:
                    {
                        Packets.ToSend.SkillUsePacket SUP = new ToSend.SkillUsePacket(Owner.UniqueID, Owner.Character.Screen.Monsters.Count + Owner.Character.Screen.Players.Count);
                        SUP.AimX = Convert.ToUInt16(x);
                        SUP.AimY = Convert.ToUInt16(y);
                        SUP.SkillID = Skill.SkillInfo.ID;
                        SUP.SkillLevel = Skill.SkillInfo.Level;
                        foreach (Entities.Monster Target in Owner.Character.Screen.Monsters.Values)
                        {
                            if (Owner.Character.CanTarget(Target, Skill))
                            {
                                uint Damage = Calculations.Attacks.MagicPlayerVsMonster(Owner.Character, Target, Skill);
                                SUP.AddTarget(Target.UniqueID, Damage);
                                Target.TakeDamage(Owner.UniqueID, Damage);
                            }
                        }
                        foreach (Network.SocketClient Target in Owner.Character.Screen.Players.Values)
                        {
                            if (Owner.Character.CanTarget(Target.Character, Skill))
                            {
                                uint Damage = Calculations.Attacks.MagicPlayerVsPlayer(Owner.Character, Target.Character, Skill);
                                SUP.AddTarget(Target.UniqueID, Damage);
                                Target.Character.TakeDamage(Owner.UniqueID, Damage);
                            }
                        }
                        if (SUP.TargetCount > 0)
                            Owner.Character.Screen.Send(SUP.ToBytes, true);
                        break;
                    }
                #endregion
                #region Sector
                case Enums.TargetType.Sector:
                    {
                        Packets.ToSend.SkillUsePacket SUP = new ToSend.SkillUsePacket(Owner.UniqueID, Owner.Character.Screen.Monsters.Count + Owner.Character.Screen.Players.Count);
                        SUP.AimX = Convert.ToUInt16(x);
                        SUP.AimY = Convert.ToUInt16(y);
                        SUP.SkillID = Skill.SkillInfo.ID;
                        SUP.SkillLevel = Skill.SkillInfo.Level;
                        foreach (Entities.Monster Target in Owner.Character.Screen.Monsters.Values)
                        {
                            if (Owner.Character.CanTarget(Target, Skill))
                            {
                                if (Calculations.InSector(Target.X, Target.Y, Owner.Character.X, Owner.Character.Y, SUP.AimX, SUP.AimY, Skill.SkillInfo.SectorSize))
                                {
                                    uint Damage = Calculations.Attacks.MagicPlayerVsMonster(Owner.Character, Target, Skill);
                                    SUP.AddTarget(Target.UniqueID, Damage);
                                    Target.TakeDamage(Owner.UniqueID, Damage);
                                }
                            }
                        }
                        foreach (Network.SocketClient Target in Owner.Character.Screen.Players.Values)
                        {
                            if (Owner.Character.CanTarget(Target.Character, Skill))
                            {
                                if (Calculations.InSector(Target.Character.X, Target.Character.Y, Owner.Character.X, Owner.Character.Y, SUP.AimX, SUP.AimY, Skill.SkillInfo.SectorSize))
                                {
                                    uint Damage = Calculations.Attacks.MagicPlayerVsPlayer(Owner.Character, Target.Character, Skill);
                                    SUP.AddTarget(Target.UniqueID, Damage);
                                    Target.Character.TakeDamage(Owner.UniqueID, Damage);
                                }
                            }
                        }
                        if (SUP.TargetCount > 0)
                            Owner.Character.Screen.Send(SUP.ToBytes, true);
                        break;
                    }
                #endregion
                #region Linear
                case Enums.TargetType.Linear:
                    {
                        Packets.ToSend.SkillUsePacket SUP = new ToSend.SkillUsePacket(Owner.UniqueID, Owner.Character.Screen.Monsters.Count + Owner.Character.Screen.Players.Count);
                        SUP.AimX = Convert.ToUInt16(x);
                        SUP.AimY = Convert.ToUInt16(y);
                        SUP.SkillID = Skill.SkillInfo.ID;
                        SUP.SkillLevel = Skill.SkillInfo.Level;
                        List<Calculations.coords> Line = new List<Calculations.coords>(Skill.SkillInfo.Range);
                        Calculations.DDALine(Owner.Character.X, Owner.Character.Y, SUP.AimX, SUP.AimY, Skill.SkillInfo.Range, ref Line);
                        foreach (Entities.Monster Target in Owner.Character.Screen.Monsters.Values)
                        {
                            if (Owner.Character.CanTarget(Target, Skill))
                            {
                                if (Calculations.InSector(Target.X, Target.Y, Owner.Character.X, Owner.Character.Y, SUP.AimX, SUP.AimY, Skill.SkillInfo.SectorSize))
                                {
                                    if (Line.FindIndex(c => c.X == Target.X && c.Y == Target.Y) >= 0)
                                    {
                                        uint Damage = Calculations.Attacks.MagicPlayerVsMonster(Owner.Character, Target, Skill);
                                        SUP.AddTarget(Target.UniqueID, Damage);
                                        Target.TakeDamage(Owner.UniqueID, Damage);
                                    }
                                }
                            }
                        }
                        foreach (Network.SocketClient Target in Owner.Character.Screen.Players.Values)
                        {
                            if (Owner.Character.CanTarget(Target.Character, Skill))
                            {
                                if (Calculations.InSector(Target.Character.X, Target.Character.Y, Owner.Character.X, Owner.Character.Y, SUP.AimX, SUP.AimY, Skill.SkillInfo.SectorSize))
                                {
                                    if (Line.FindIndex(c => c.X == Target.Character.X && c.Y == Target.Character.Y) >= 0)
                                    {
                                        uint Damage = Calculations.Attacks.MagicPlayerVsPlayer(Owner.Character, Target.Character, Skill);
                                        SUP.AddTarget(Target.UniqueID, Damage);
                                        Target.Character.TakeDamage(Owner.UniqueID, Damage);
                                    }
                                }
                            }
                        }
                        if (SUP.TargetCount > 0)
                            Owner.Character.Screen.Send(SUP.ToBytes, true);
                        break;
                    }
                #endregion
                case Enums.TargetType.FromPoint: break; //useless
                case Enums.TargetType.FromSingle: break; //Todo
            }
        }
    }
}
