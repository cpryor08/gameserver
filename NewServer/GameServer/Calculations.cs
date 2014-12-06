using System;
using System.Collections.Generic;
namespace GameServer
{
    public class Calculations
    {
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }
        public static double Direction(double x1, double y1, double x2, double y2)
        {
            double direction = 0;

            double AddX = x2 - x1;
            double AddY = y2 - y1;
            double r = (double)Math.Atan2(AddY, AddX);

            if (r < 0) r += (double)Math.PI * 2;

            direction = 360 - (r * 180 / (double)Math.PI);
            return direction;
        }
        public static int AttackMultiplier(byte AttackerLevel, byte AttackedLevel)
        {
            if (AttackedLevel >= AttackerLevel)
                return 1;
            return (AttackerLevel - AttackedLevel) / 10 + 1;
        }
        public static Enums.MobNameType GetNameType(byte PlayerLevel, byte MonsterLevel)
        {
            if (PlayerLevel >= MonsterLevel)
            {
                byte dif = (byte)(PlayerLevel - MonsterLevel);
                if (dif >= 3)
                    return Enums.MobNameType.Green;
                else
                    return Enums.MobNameType.White;
            }
            else
            {
                byte dif = (byte)(MonsterLevel - PlayerLevel);
                if (dif <= 4)
                    return Enums.MobNameType.Red;
                else
                    return Enums.MobNameType.Black;
            }
        }
        public static void CalculateMaxHP(Entities.Character Character)
        {
            double HP = (Character.Vitality * 24) + ((Character.Strength + Character.Agility + Character.Spirit) * 3) + 1;
            switch (Character.Class)
            {
                case 11:
                    HP *= 1.05;
                    break;
                case 12:
                    HP *= 1.08;
                    break;
                case 13:
                    HP *= 1.10;
                    break;
                case 14:
                    HP *= 1.12;
                    break;
                case 15:
                    HP *= 1.15;
                    break;
                default: break;
            }
            HP += Character.BonusHPByItems;
            Character.MaxHitPoints = (ushort)HP;
        }
        public static void CalculateMaxMana(Entities.Character Character)
        {
            byte manaBoost = 5;
            sbyte JobID = (sbyte)(Character.Class / 10);
            if (JobID == 13 || JobID == 14)
            {
                manaBoost += (byte)(5 * (Character.Class - (JobID * 10)));
            }

            Character.MaxMana = (ushort)(Character.Spirit * manaBoost);
        }
        public static int ReqProfExp(byte Level, byte CharLevel)
        {
            if (Level == 1)
                return 1200;
            else if (Level == 2 && CharLevel >= 10)
                return 68000;
            else if (Level == 3 && CharLevel >= 20)
                return 250000;
            else if (Level == 4 && CharLevel >= 30)
                return 640000;
            else if (Level == 5 && CharLevel >= 40)
                return 1600000;
            else if (Level == 6 && CharLevel >= 50)
                return 4000000;
            else if (Level == 7 && CharLevel >= 60)
                return 10000000;
            else if (Level == 8 && CharLevel >= 70)
                return 22000000;
            else if (Level == 9 && CharLevel >= 80)
                return 40000000;
            else if (Level == 10 && CharLevel >= 90)
                return 90000000;
            else if (Level == 11 && CharLevel >= 100)
                return 95000000;
            else if (Level == 12 && CharLevel >= 110)
                return 142500000;
            else if (Level == 13 && CharLevel >= 110)
                return 213750000;
            else if (Level == 14 && CharLevel >= 110)
                return 320625000;
            else if (Level == 15 && CharLevel >= 110)
                return 480937500;
            else if (Level == 16 && CharLevel >= 110)
                return 721406250;
            else if (Level == 17 && CharLevel >= 110)
                return 1082109375;
            else if (Level == 18 && CharLevel >= 110)
                return 1623164063;
            else if (Level == 19 && CharLevel >= 110)
                return 2100000000;
            else
                return -1;
        }
        public static int GetDropID(byte Level)
        {
            int ItemID = 0;
            int Random = Program.Random.Next(0, 70);
            #region Headgear
            if (Random <= 10) //Headgear
            {
                int Lvl;
                if (Level >= 20)
                {
                    Lvl = Math.Min(90, (int)Level);
                    Lvl = Lvl - (Lvl % 10);
                }
                else
                    Lvl = 1;

                Random = Program.Random.Next(0, 100);
                if (Random < 20) //Warrior Helmet
                    ItemID = 111000;
                else if (Random < 40) //Trojan Coronet
                    ItemID = 118000;
                else if (Random < 60) //Taoist Cap
                    ItemID = 114000;
                else if (Random < 80) //Archer Hat
                    ItemID = 113000;
                if (Random < 80)
                    ItemID += (Program.Random.Next(3, 9) * 100);
                else
                    ItemID = 117300; //Earrings
                ItemID += Lvl;
            }
            #endregion
            #region Necklace/Bag
            else if (Random < 20)
            {
                int Lvl;
                if (Level >= 20)
                {
                    Lvl = Math.Min(100, (int)Level);
                    Lvl = Lvl - (Lvl % 10);
                }
                else
                    Lvl = 1;
                if (Lvl < 50)
                    Lvl /= 5;
                else if (Lvl == 50)
                    Lvl = 9;
                else
                {
                    Lvl -= 30;
                    Lvl /= 10;
                    Lvl *= 3;
                }

                Random = Program.Random.Next(0, 100);
                if (Random < 80)
                    ItemID = 120000; //Necklace
                else
                    ItemID = 121000; //Bag
                ItemID += (Lvl * 10);
            }
            #endregion
            #region Attack/Heavy Ring
            else if (Random < 30)
            {
                int Lvl;
                if (Level >= 20)
                {
                    Lvl = Math.Min(110, (int)Level);
                    Lvl = Lvl - (Lvl % 10);
                }
                else
                    Lvl = 1;

                Random = Program.Random.Next(0, 100);
                if (Random < 50)
                    ItemID = 150000; //Attack Ring
                else
                    ItemID = 151000; //Heavy Ring
                int xTra = 1;
                Lvl /= 10;
                for (int i = 1; i < Lvl; i++)
                    xTra += 2;
                ItemID += xTra * 10;
            }
            #endregion
            #region Bracelet
            else if (Random < 35)
            {
                int Lvl;
                if (Level >= 20)
                {
                    Lvl = Math.Min(100, (int)Level);
                    Lvl = Lvl - (Lvl % 10);

                    ItemID = 152000 + (((Lvl / 10) * 2) * 10); //Bracelet
                }
                else
                    ItemID = 152010;
            }
            #endregion
            #region Armor
            else if (Random < 50)
            {
                int Lvl;
                if (Level >= 20)
                {
                    Lvl = Math.Min(90, (int)Level);
                    Lvl = Lvl - (Lvl % 10);
                }
                else
                    Lvl = 1;

                Random = Program.Random.Next(0, 80);
                if (Random < 20)
                    ItemID = 133000; //Archer Coat
                else if (Random < 40)
                    ItemID = 134000; //Taoist Robe
                else if (Random < 60)
                    ItemID = 131000; //Warrior Armor
                else
                    ItemID = 130000; //Tro Armor
                ItemID += (Program.Random.Next(3, 9) * 100) + Lvl;
            }
            #endregion
            #region Boots
            else if (Random < 60)
            {
                int Lvl;
                if (Level >= 20)
                {
                    Lvl = Math.Min(110, (int)Level);
                    Lvl = Lvl - (Lvl % 10);
                }
                else
                    Lvl = 1;

                int xTra = 1;
                Lvl /= 10;
                for (int i = 1; i < Lvl; i++)
                    xTra += 2;
                ItemID = 160000 + (xTra * 10);
            }
            #endregion
            #region Weapons
            else
            {
                int Lvl;
                if (Level >= 20)
                {
                    Lvl = Math.Min(110, (int)Level);
                    Lvl = Lvl - (Lvl % 10);
                    Lvl /= 10;
                    Lvl *= 2;
                    Random = Program.Random.Next(0, 15);
                    if (Random < 5)
                        Lvl--;
                    else if (Random < 10)
                        Lvl++;
                    Lvl = Math.Min(21, Lvl);
                }
                else
                {
                    Lvl = Level - (Level % 10);
                    Lvl /= 10;
                }
                Lvl = Lvl / 10;

                Random = Program.Random.Next(0, 160);
                if (Random < 10)
                    ItemID = 580000; //Halbert
                else if (Random < 20)
                    ItemID = 561000; //Wand
                else if (Random < 30)
                    ItemID = 440000; //Whip
                else if (Random < 40)
                    ItemID = 510000; //Glaive
                else if (Random < 50)
                    ItemID = 481000; //Scepter
                else if (Random < 60)
                    ItemID = 480000; //Club
                else if (Random < 70)
                    ItemID = 420000; //Sword
                else if (Random < 80)
                    ItemID = 421000; //Backsword
                else if (Random < 90)
                    ItemID = 410000; //Blade
                else if (Random < 100)
                    ItemID = 560000; //Spear
                else if (Random < 110)
                    ItemID = 430000; //Hook
                else if (Random < 115)
                    ItemID = 460000; //Hammer
                else if (Random < 120)
                    ItemID = 540000; //Hammer
                else if (Random < 130)
                    ItemID = 530000; //Poleaxe
                else if (Random < 140)
                    ItemID = 490000; //Dagger
                else if (Random < 150)
                    ItemID = 450000; //Axe
                else
                    ItemID = 500000; //Bow
                ItemID += Lvl * 10;
            }
            #endregion
            #region Shield
            if (Level >= 40 && Level < 100)
            {
                if (Program.Random.Next(0, 100) < 5)
                {
                    int Lvl = Level - (Level % 10);
                    Lvl -= Program.Random.Next(0, 4) * 10;
                    Lvl = Math.Min(90, Lvl);
                    ItemID = 900000 + (Program.Random.Next(3, 9) * 100) + Lvl;
                }
            }
            #endregion
            ItemID += Program.Random.Next(5, 9);
            return ItemID;
        }
        public static byte GetPlus()
        {
            if (Program.Random.Next(0, 1000) < 70)
                return 1;
            return 0;
        }
        private static int ToDegree(int Val)
        {
            if (Val > 359)
                Val -= 360;
            else if (Val < 0)
                Val += 360;
            return Val;
        }
        public static bool InSector(ushort X, ushort Y, ushort UserX, ushort UserY, ushort AimX, ushort AimY, int SectorSize)
        {
            int Aim = (int)Calculations.Direction(UserX, UserY, AimX, AimY);
            int MobAngle = (int)Calculations.Direction(UserX, UserY, X, Y);
            int Half = SectorSize / 2;
            int Start = Aim - Half;
            int End = Aim + Half;
            if (Start < 0)
            {
                Start = ToDegree(Start);
                End = ToDegree(End);
                MobAngle = ToDegree(MobAngle);
                if (Start > 180)
                    if (MobAngle < End || MobAngle > Start)
                        return true;
                if (MobAngle >= Start && MobAngle <= End)
                    return true;
            }
            if (End > 360)
            {
                Start = ToDegree(Start);
                End = ToDegree(End);
                MobAngle = ToDegree(MobAngle);
                if (End < 180)
                    if (MobAngle < End || MobAngle > Start)
                        return true;
                if (MobAngle >= Start && MobAngle <= End)
                    return true;
            }
            if (MobAngle >= Start && MobAngle <= End)
                return true;
            return false;
        }
        public struct coords
        {
            public int X;
            public int Y;

            public coords(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        public static void DDALine(int x0, int y0, int x1, int y1, int nRange, ref List<coords> vctPoint)
        {
            if (x0 == x1 && y0 == y1)
                return;

            float scale = (float)(1.0f * nRange / Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0)));
            x1 = (int)(0.5f + scale * (x1 - x0) + x0);
            y1 = (int)(0.5f + scale * (y1 - y0) + y0);
            DDALineEx(x0, y0, x1, y1, ref vctPoint);
        }
        public static void DDALineEx(int x0, int y0, int x1, int y1, ref List<coords> vctPoint)
        {
            if (x0 == x1 && y0 == y1)
                return;
            int dx = x1 - x0;
            int dy = y1 - y0;
            int abs_dx = Math.Abs(dx);
            int abs_dy = Math.Abs(dy);
            coords point = new coords();
            if (abs_dx > abs_dy)
            {
                int _0_5 = abs_dx * (dy > 0 ? 1 : -1);
                int numerator = dy * 2;
                int denominator = abs_dx * 2;
                if (dx > 0)
                {
                    for (int i = 1; i <= abs_dx; i++)
                    {
                        point = new coords();
                        point.X = x0 + i;
                        point.Y = y0 + ((numerator * i + _0_5) / denominator);
                        vctPoint.Add(point);
                    }
                }
                else if (dx < 0)
                {
                    for (int i = 1; i <= abs_dx; i++)
                    {
                        point = new coords();
                        point.X = x0 - i;
                        point.Y = y0 + ((numerator * i + _0_5) / denominator);
                        vctPoint.Add(point);
                    }
                }
            }
            else
            {
                int _0_5 = abs_dy * (dx > 0 ? 1 : -1);
                int numerator = dx * 2;
                int denominator = abs_dy * 2;
                if (dy > 0)
                {
                    for (int i = 1; i <= abs_dy; i++)
                    {
                        point = new coords();
                        point.Y = y0 + i;
                        point.X = x0 + ((numerator * i + _0_5) / denominator);
                        vctPoint.Add(point);
                    }
                }
                else if (dy < 0)
                {
                    for (int i = 1; i <= abs_dy; i++)
                    {
                        point = new coords();
                        point.Y = y0 - i;
                        point.X = x0 + ((numerator * i + _0_5) / denominator);
                        vctPoint.Add(point);
                    }
                }
            }
        }
        public class Attacks
        {
            public static uint RangedPlayerVsMonster(Entities.Character Attacker, Entities.Monster Attacked)
            {
                uint Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                Damage = (uint)((double)Damage * ((double)(210 - Attacked.Info.Dodge) / 100));
                Damage *= (uint)(Math.Min(1, Damage * (Attacked.Info.Dodge * .01)));
                if (Attacker.Level > Attacked.Info.Level)
                {
                    int dLvl = Attacker.Level - Attacked.Info.Level;
                    if (dLvl >= 3 && dLvl <= 5)
                        Damage = (uint)(Damage * 1.5);
                    else if (dLvl > 5 && dLvl <= 10)
                        Damage *= 2;
                    else if (dLvl > 10 && dLvl <= 20)
                        Damage *= 3;
                    else if (dLvl > 20)
                        Damage = (uint)(Damage * (1 + (.3 * dLvl / 5)));
                }
                return Math.Max(1, Damage);
            }
            public static uint RangedPlayerVsPlayer(Entities.Character Attacker, Entities.Character Attacked)
            {
                uint Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                int _dodge = Attacked.Dodge;
                Damage = (uint)((double)Damage * (1 - ((double)_dodge * .01)));
                Damage = (uint)((double)Damage * .15);
                return Math.Max(1, Damage);
            }
            public static uint MeleePlayerVsMonster(Entities.Character Attacker, Entities.Monster Attacked)
            {
                uint Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                if (Attacked.Info.Defense >= Damage)
                    Damage = 1;
                else
                    Damage -= Attacked.Info.Defense;
                if (Attacker.Level > Attacked.Info.Level)
                {
                    int dLvl = Attacker.Level - Attacked.Info.Level;
                    if (dLvl >= 3 && dLvl <= 5)
                        Damage = (uint)(Damage * 1.5);
                    else if (dLvl > 5 && dLvl <= 10)
                        Damage *= 2;
                    else if (dLvl > 10 && dLvl <= 20)
                        Damage *= 3;
                    else if (dLvl > 20)
                        Damage = (uint)(Damage * (1 + (.3 * dLvl / 5)));
                }
                return Math.Max(1, Damage);
            }
            public static uint MeleePlayerVsPlayer(Entities.Character Attacker, Entities.Character Attacked)
            {
                uint Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                if (Attacked.PDefense >= Damage)
                    Damage = 1;
                else
                    Damage -= Attacked.PDefense;
                return Math.Max(1, Damage);
            }
            public static uint MagicPlayerVsMonster(Entities.Character Attacker, Entities.Monster Attacked, Skills.Skill Skill)
            {
                uint Damage = 0;
                switch(Skill.SkillInfo.DamageType)
                {
                    #region Magic
                    case Enums.DamageType.Magic:
                        {
                            Damage = Skill.SkillInfo.Damage;
                            Damage = (uint)((double)(Damage + Attacker.MagicAttack) * (1 + ((double)Attacker.PhoenixGemBonus / 100)));
                            if (Attacked.Info.MagicDefense >= Damage)
                                Damage = 1;
                            else
                                Damage -= Attacked.Info.MagicDefense;
                            Damage = (uint)((double)(Damage * Skill.SkillInfo.EffectValue));
                            break;
                        }
                    #endregion
                    #region Ranged
                    case Enums.DamageType.Ranged:
                        {
                            Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                            Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                            Damage = (uint)((double)Damage * ((double)(210 - Attacked.Info.Dodge) / 100));
                            Damage = (uint)((double)Damage * Skill.SkillInfo.EffectValue);
                            Damage = (uint)(Math.Min(1, Damage * (Attacked.Info.Dodge * .01)));
                            break;
                        }
                    #endregion
                    #region Melee
                    case Enums.DamageType.Melee:
                        {
                            Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                            Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                            if (Attacked.Info.Defense >= Damage)
                                Damage = 1;
                            else
                                Damage -= Attacked.Info.Defense;
                            Damage = (uint)((double)Damage * Skill.SkillInfo.EffectValue);
                            break;
                        }
                    #endregion
                    #region HealHP
                    case Enums.DamageType.HealHP: return Skill.SkillInfo.Damage;
                    #endregion
                    #region HealMP
                    case Enums.DamageType.HealMP: return Skill.SkillInfo.Damage;
                    #endregion
                    default: break;
                }
                if (Attacker.Level > Attacked.Info.Level)
                {
                    int dLvl = Attacker.Level - Attacked.Info.Level;
                    if (dLvl >= 3 && dLvl <= 5)
                        Damage = (uint)(Damage * 1.5);
                    else if (dLvl > 5 && dLvl <= 10)
                        Damage *= 2;
                    else if (dLvl > 10 && dLvl <= 20)
                        Damage *= 3;
                    else if (dLvl > 20)
                        Damage = (uint)(Damage * (1 + (.3 * dLvl / 5)));
                }
                return Math.Max(1, Damage);
            }
            public static uint MagicPlayerVsPlayer(Entities.Character Attacker, Entities.Character Attacked, Skills.Skill Skill)
            {
                uint Damage = 0;
                switch (Skill.SkillInfo.DamageType)
                {
                    #region Magic
                    case Enums.DamageType.Magic:
                        {
                            Damage = Skill.SkillInfo.Damage;
                            Damage = (uint)((double)(Damage + Attacker.MagicAttack) * (1 + ((double)Attacker.PhoenixGemBonus / 100)));
                            Damage = (uint)((double)Damage * (1 - ((double)Attacked.MDefenseByPct / 100)));
                            if (Attacked.MDefenseByVal >= Damage)
                                Damage = 1;
                            else
                                Damage -= Attacked.MDefenseByVal;
                            Damage = (uint)((double)(Damage * Skill.SkillInfo.EffectValue));
                            break;
                        }
                    #endregion
                    #region Ranged
                    case Enums.DamageType.Ranged:
                        {
                            Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                            Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                            Damage = (uint)((double)Damage * Skill.SkillInfo.EffectValue);
                            Damage = (uint)((double)Damage * ((double)(210 - Attacked.Dodge) / 100));
                            Damage = (uint)(Math.Min(1, Damage * (Attacked.Dodge * .01)));
                            Damage = (uint)((double)Damage * .15);
                            break;
                        }
                    #endregion
                    #region Melee
                    case Enums.DamageType.Melee:
                        {
                            Damage = (uint)Program.Random.Next(Attacker.MinAttack, Attacker.MaxAttack);
                            Damage = (uint)((double)Damage * (1 + (Attacker.DragonGemBonus / 100)));
                            Damage = (uint)((double)Damage * Skill.SkillInfo.EffectValue);
                            if (Attacked.PDefense >= Damage)
                                Damage = 1;
                            else
                                Damage -= Attacked.PDefense;
                            break;
                        }
                    #endregion
                    #region HealHP
                    case Enums.DamageType.HealHP: return Skill.SkillInfo.Damage;
                    #endregion
                    #region HealMP
                    case Enums.DamageType.HealMP: return Skill.SkillInfo.Damage;
                    #endregion
                    default: break;
                }
                return Math.Max(1, Damage);
            }
        }
    }
}
