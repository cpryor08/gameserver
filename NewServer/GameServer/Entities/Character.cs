using System;
using GameServer.Network;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace GameServer.Entities
{
    public class Character
    {
        private uint UniqueID;
        private Packets.Writer PacketWriter;
        public Mapping.Screen Screen;
        public Attacking.AttackHandler AttackHandler;
        public NPCs.NPCHandler CurrentDialog;
        private SocketClient Owner;
        public Buffs.BuffCollection Buffs;
        public Character(uint UniqueID, SocketClient socClient)
        {
            this.Owner = socClient;
            this.UniqueID = UniqueID;
            this.Screen = new Mapping.Screen(socClient);
            this.AttackHandler = new Attacking.AttackHandler(socClient);
            this.CurrentDialog = new NPCs.NPCHandler(socClient);
            this.Inventory = new Items.Inventory(socClient);
            this.Equipment = new Items.Equipment(socClient);
            PacketWriter = new Packets.Writer(102);
            PacketWriter.Fill((ushort)102, 0);
            PacketWriter.Fill((ushort)1014, 2);
            PacketWriter.Fill(UniqueID, 4);
            PacketWriter.Fill((byte)1, 80);
        }
        ~Character()
        {
            Owner.Disconnect();
            if (TeamID > 0)
            {
                Team T;
                if (Kernel.Teams.TryGetValue(TeamID, out T))
                    T.RemoveMember(UniqueID);
            }
            Screen.Clear();
            Skills.Clear();
            Profs.Clear();
            Inventory.Items.Clear();
            Equipment.Clear();
            Warehouses.Clear();
            Buffs.Clear();
        }
        #region BaseValues
        private string _name;

        private byte _rbcount;
        private byte _class;
        private byte _level;
        private Enums.ConquerAngle _facing;

        private ushort _pkpoints;
        private ushort _hitpoints;
        private ushort _mana;
        private ushort _hairstyle;
        private ushort _avatar;
        private ushort _trans;
        private ushort _x;
        private ushort _y;
        private ushort _strength;
        private ushort _agility;
        private ushort _spirit;
        private ushort _vitality;
        private ushort _statpoints;

        private uint _fullmodel;
        private uint _experience;
        private uint _spouse;
        private uint _model;
        private uint _mapid;
        private uint _silver;
        private uint _storedsilver;
        private uint _cps;
        private uint _virtuepoints;
        #endregion

        #region Info
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PacketWriter.Fill((byte)_name.Length, 81);
                PacketWriter.Fill(_name, 82);
                if (Owner.Connected)
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `info` SET `name`='" + _name + "' WHERE `UniqueID`=" + UniqueID);
            }
        }

        public byte RBCount
        {
            get { return _rbcount; }
            set
            {
                _rbcount = value;
                PacketWriter.Fill(_rbcount, 60);
                if (Owner.Connected)
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `info` SET `RBCount`=" + _rbcount + " WHERE `UniqueID`=" + UniqueID);
            }
        }
        public byte Class
        {
            get { return _class; }
            set
            {
                _class = value;
                if (Owner.Connected)
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `info` SET `class`=" + _class + " WHERE `UniqueID`=" + UniqueID);
            }
        }
        public byte Level
        {
            get { return _level; }
            set
            {
                _level = value;
                PacketWriter.Fill(_level, 50);
                PacketWriter.Fill(_level, 62);
                if (Owner.Connected)
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `info` SET `level`=" + _level + " WHERE `UniqueID`=" + UniqueID);
            }
        }
        public Enums.ConquerAngle Facing
        {
            get { return _facing; }
            set
            {
                _facing = value;
                PacketWriter.Fill((byte)_facing, 58);
                if (Owner.Connected)
                    Screen.Send(Packets.ToSend.GeneralData(Owner.UniqueID, 0, 0, 0, (byte)_facing, Enums.GeneralData.ChangeDirection), true);
            }
        }

        private Enums.PKMode _pkmode = Enums.PKMode.Capture;
        public Enums.PKMode PKMode
        {
            get { return _pkmode; }
            set
            {
                _pkmode = value;
                if (Owner.Connected)
                    Owner.Send(Packets.ToSend.GeneralData(UniqueID, (ushort)_pkmode, 0, 0, 0, Enums.GeneralData.ChangePkMode));
            }
        }
        public ushort PKPoints
        {
            get { return _pkpoints; }
            set
            {
                _pkpoints = value;
                if (Owner.Connected)
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `info` SET `pkpoints`=" + _pkpoints + " WHERE `UniqueID`=" + UniqueID);
            }
        }
        public ushort MaxHitPoints;
        public ushort HitPoints
        {
            get { return _hitpoints; }
            set
            {
                _hitpoints = value;
                PacketWriter.Fill(_hitpoints, 48);
                if (Owner.Connected)
                    Owner.Send(Packets.ToSend.SyncPacket(UniqueID, Enums.SyncType.Hitpoints, _hitpoints));
            }
        }
        public ushort MaxMana;
        public ushort Mana;
        public byte Stamina;

        public uint Experience;
        public uint Spouse;
        public uint TeamID;
        #endregion

        #region Appearance
        public ushort Transformation
        {
            get { return _trans; }
            set
            {
                _trans = value;
                FullModel = (uint)(value * 10000000 + Avatar * 10000 + Model);
            }
        }
        public ushort HairStyle
        {
            get { return _hairstyle; }
            set
            {
                _hairstyle = value;
                PacketWriter.Fill(_hairstyle, 56);
                if (Owner.Connected)
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `appearance` SET `hairstyle`=" + _hairstyle + " WHERE `UniqueID`=" + UniqueID);
            }
        }
        public ushort Avatar
        {
            get { return _avatar; }
            set
            {
                _avatar = value;
                FullModel = (uint)(Transformation * 10000000 + value * 10000 + Model);
            }
        }
        public uint Model
        {
            get { return _model; }
            set
            {
                _model = value;
                FullModel = (uint)(Transformation * 10000000 + Avatar * 10000 + value);
            }
        }
        public uint FullModel
        {
            get { return _fullmodel; }
            set
            {
                _fullmodel = value;
                if (Owner.Connected)
                    Screen.Send(Packets.ToSend.SyncPacket(Owner.UniqueID, Enums.SyncType.Mesh, _fullmodel), true);
            }
        }
        #endregion

        #region Location
        public ushort X
        {
            get { return _x; }
            set
            {
                _x = value;
                PacketWriter.Fill(_x, 52);
            }
        }
        public ushort Y
        {
            get { return _y; }
            set
            {
                _y = value;
                PacketWriter.Fill(_y, 54);
            }
        }

        public Mapping.Map Map;
        public uint MapID
        {
            get { return Map.UniqueID; }
            set
            {
                Kernel.Maps.TryGetValue(value, out Map);
            }
        }
        public int MapKeyX = 255;
        public int MapKeyY = 255;

        public void Walk(Enums.ConquerAngle Direction)
        {
            int xi = 0, yi = 0;
            switch (Direction)
            {
                case Enums.ConquerAngle.North: xi = -1; yi = -1; break;
                case Enums.ConquerAngle.South: xi = 1; yi = 1; break;
                case Enums.ConquerAngle.East: xi = 1; yi = -1; break;
                case Enums.ConquerAngle.West: xi = -1; yi = 1; break;
                case Enums.ConquerAngle.NorthWest: xi = -1; break;
                case Enums.ConquerAngle.SouthWest: yi = 1; break;
                case Enums.ConquerAngle.NorthEast: yi = -1; break;
                case Enums.ConquerAngle.SouthEast: xi = 1; break;
            }
            X += (ushort)xi;
            Y += (ushort)yi;
        }
        #endregion

        #region Money
        public uint Silver
        {
            get { return _silver; }
            set
            {
                _silver = value;
                if (Owner.Connected)
                {
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `money` SET `Silver`=" + _silver + " WHERE `UniqueID`=" + Owner.UniqueID);
                    Owner.Send(Packets.ToSend.SyncPacket(Owner.UniqueID, Enums.SyncType.Money, _silver));
                }
            }
        }
        public uint StoredSilver
        {
            get { return _storedsilver; }
            set
            {
                _storedsilver = value;
                if (Owner.Connected)
                {
                    Database.Database.CharacterDB.ExecuteNonQuery("UPDATE `money` SET `StoredSilver`=" + _storedsilver + " WHERE `UniqueID`=" + Owner.UniqueID);
                    Owner.Send(Packets.ToSend.ItemUsage(Owner.UniqueID, (byte)Enums.ItemUsage.ViewWarehouse, _storedsilver, 0));
                }
            }
        }
        public uint CPs;
        public uint VirtuePoints;
        #endregion

        #region Stats
        public ushort Strength;
        public ushort Agility;
        public ushort Spirit;
        public ushort Vitality;
        public ushort StatPoints;
        #endregion

        #region Combat Stats
        public byte AttackRange = 2;
        public int MinAttack;
        public int MaxAttack;
        public int MagicAttack;
        public uint MDefenseByPct;
        public uint MDefenseByVal;
        public uint PDefense;
        public byte Dodge;
        public ushort BonusHPByItems;
        public double PhoenixGemBonus;
        public double DragonGemBonus;
        #endregion

        #region Items
        public Items.Equipment Equipment;
        public Items.Inventory Inventory;
        public ConcurrentDictionary<uint, ConcurrentBag<Items.Item>> Warehouses = new ConcurrentDictionary<uint, ConcurrentBag<Items.Item>>();
        #endregion

        #region Skills
        public ConcurrentDictionary<ushort, Skills.Skill> Skills = new ConcurrentDictionary<ushort, Skills.Skill>();
        public void LearnSkill(ushort SkillID, byte Level, uint Experience)
        {
            if (!Kernel.SkillInfos.ContainsKey(SkillID))
                return;
            Skills.SkillInfo SI;
            if (!Kernel.SkillInfos[SkillID].TryGetValue(Level, out SI))
                return;
            Skills.Skill s = new Skills.Skill(SI);
            s.Experience = Experience;
            if (Skills.TryAdd(SkillID, s))
                Owner.Send(Packets.ToSend.SpellPacket(SkillID, Level, Experience));
        }
        public void ForgetSkill(ushort SkillID)
        {
            Skills.Skill s;
            if (Skills.TryRemove(SkillID, out s))
                Owner.Send(Packets.ToSend.GeneralData(Owner.UniqueID, SkillID, 0, 0, 0, Enums.GeneralData.ForgetSpell));
        }
        public void GainSkillExperience(ushort SkillID, uint Amount)
        {
            Skills.Skill currentSkill;
            if (!Skills.TryRemove(SkillID, out currentSkill))
                return;
            if (currentSkill.SkillInfo.UpgReqExp == 0)
                return;
            if (Level < currentSkill.SkillInfo.UpgReqLev)
                return;
            switch (SkillID)
            {
                case 1000:
                case 1001:
                case 1002:
                    Amount *= 10;
                    break;
                case 1290:
                    break;
                case 1195:
                    {
                        //if (Mana == MaxMana)
                        //    Amount = 0;
                        //else
                            Amount = 3250;
                        break;
                    }
                case 5030:
                    Amount = 15; break;
                case 7030:
                    Amount = 100; break;
                case 7020: Amount = (uint)((double)Amount * 1.5); break;
                case 8001:
                    Amount = 10000; break;
                case 9000: Amount = 50; break;
            }
            /*
            Amount += (uint)((double)Amount * (Hero.MagicExperienceBonus / 100));
            S.Experience += Amount;
            if (S.Experience >= S.UpgReqExp)
            {
                if (Entry.Spells[Spell.ID].SpellDetails.TryGetValue((byte)(Spell.Level + 1), out Spell))
                {
                    Remove(Spell.ID);
                    Spell.Experience = 0;
                    Add(Spell.ID, Spell.Level, Spell.Experience, true);
                    Entry.Database.UpdateSkill(Spell, Hero);
                }
            }
             * */
            if (Skills.TryAdd(SkillID, currentSkill))
                Owner.Send(Packets.ToSend.SpellPacket(SkillID, currentSkill.SkillInfo.Level, currentSkill.Experience));
            else
                Owner.Send(Packets.ToSend.GeneralData(Owner.UniqueID, SkillID, 0, 0, 0, Enums.GeneralData.ForgetSpell));
        }
        #endregion

        #region Profs
        public ConcurrentDictionary<ushort, Profs.Prof> Profs = new ConcurrentDictionary<ushort, Profs.Prof>();
        #endregion

        public bool Dead { get { return HitPoints == 0; } }

        public bool CanTarget(Entities.Character Target)
        {
            if (Target.Dead)
                return false;
            if (MapID != Target.MapID)
                return false;
            if (UniqueID == Target.UniqueID)
                return false;
            if (Calculations.GetDistance(X, Y, Target.X, Target.Y) > AttackRange)
                return false;
            switch (PKMode)
            {
                case Enums.PKMode.Peace: return false;
                case Enums.PKMode.Capture:
                    {
                        if (Target.Buffs.ContainsFlag(Enums.Flag.Flashing) || Target.Buffs.ContainsFlag(Enums.Flag.BlackName))
                            return true;
                        return false;
                    }
                case Enums.PKMode.Team:
                    {
#warning Add Team/Guild Check
                        return false;
                    }
                case Enums.PKMode.PK: return true;
                default: return false;
            }
        }
        public bool CanTarget(Entities.Character Target, Skills.Skill Skill)
        {
            if (Target.Dead && Skill.SkillInfo.ExtraEffect != Enums.SkillExtraEffect.Revive)
                return false;
            if (MapID != Target.MapID)
                return false;
            if (UniqueID == Target.UniqueID && !Skill.SkillInfo.CanTargetSelf)
                return false;
            if (Calculations.GetDistance(X, Y, Target.X, Target.Y) > Skill.SkillInfo.Range)
                return false;
            if (Skill.SkillInfo.DamageType == Enums.DamageType.HealHP || Skill.SkillInfo.DamageType == Enums.DamageType.HealMP)
                return true;
            if (Skill.SkillInfo.ExtraEffect != Enums.SkillExtraEffect.None)
                return true;
            switch(PKMode)
            {
                case Enums.PKMode.Peace: return false;
                case Enums.PKMode.Capture:
                    {
                        if (Target.Buffs.ContainsFlag(Enums.Flag.Flashing) || Target.Buffs.ContainsFlag(Enums.Flag.BlackName))
                            return true;
                        return false;
                    }
                case Enums.PKMode.Team:
                    {
#warning Add Team/Guild Check
                        return false;
                    }
                case Enums.PKMode.PK: return true;
                default: return false;
            }
        }
        public bool CanTarget(Entities.Monster Target)
        {
            if (Target.HitPoints == 0)
                return false;
            if (Calculations.GetDistance(X, Y, Target.X, Target.Y) > AttackRange)
                return false;
            return true;
        }
        public bool CanTarget(Entities.Monster Target, Skills.Skill Skill)
        {
            if (Target.HitPoints == 0)
                return false;
            if (Calculations.GetDistance(X, Y, Target.X, Target.Y) > Skill.SkillInfo.Range)
                return false;

            if (Skill.SkillInfo.DamageType == Enums.DamageType.HealHP)
                return true;
            else if (Skill.SkillInfo.DamageType == Enums.DamageType.HealMP)
                return false;

            if (Skill.SkillInfo.ExtraEffect == Enums.SkillExtraEffect.Poison)
                return true;
            else if (Skill.SkillInfo.ExtraEffect != Enums.SkillExtraEffect.None)
                return false;

            return true;
        }
        public void TakeDamage(uint AttackerUID, uint Damage)
        {
            if (HitPoints > Damage)
                HitPoints = (ushort)(HitPoints - Damage);
            else
            {
                HitPoints = 0;
                Screen.Send(Packets.ToSend.Attack(AttackerUID, UniqueID, X, Y, Damage, Enums.AttackType.Death), true);
                Die();
            }
        }

        public void Die()
        {

        }
        public void Save()
        {
            Database.Methods.SaveUserInfo(UniqueID, Experience, HitPoints, Mana, PKPoints);
            Database.Methods.SaveLocation(UniqueID, Map.UniqueID, X, Y);
            Database.Methods.GenerateItemKeyFile(Owner);
        }

        public byte[] ToBytes() { return PacketWriter.Bytes; }
    }
}
