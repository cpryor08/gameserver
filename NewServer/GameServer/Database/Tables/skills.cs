using System;
using System.IO;
using System.Data;
using GameServer.Network;
using GameServer.Skills;
using System.Collections.Concurrent;
namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadSkills()
        {
            string[] Skills = File.ReadAllLines("C:/db/skills.dat");
            for (int i = 0; i < Skills.Length; i++)
            {
                string[] SkillInfo = Skills[i].Split(' ');
                SkillInfo SI = new SkillInfo();
                SI.ID = ushort.Parse(SkillInfo[0]);
                SI.Level = byte.Parse(SkillInfo[1]);
                SI.ActivationChance = byte.Parse(SkillInfo[2]);
                SI.ArrowCost = byte.Parse(SkillInfo[3]);
                SI.Damage = uint.Parse(SkillInfo[4]);
                SI.DamageType = (Enums.DamageType)byte.Parse(SkillInfo[5]);
                SI.EffectLasts = ushort.Parse(SkillInfo[6]);
                SI.EffectValue = float.Parse(SkillInfo[7]);
                SI.EndsXPWait = byte.Parse(SkillInfo[8]) == 1 ? true : false;
                SI.ExtraEffect = (Enums.SkillExtraEffect)byte.Parse(SkillInfo[9]);
                SI.ManaCost = ushort.Parse(SkillInfo[10]);
                SI.Range = byte.Parse(SkillInfo[11]);
                SI.SectorSize = byte.Parse(SkillInfo[12]);
                SI.StaminaCost = byte.Parse(SkillInfo[13]);
                SI.TargetType = (Enums.TargetType)byte.Parse(SkillInfo[14]);
                SI.UpgReqExp = uint.Parse(SkillInfo[15]);
                SI.UpgReqLev = byte.Parse(SkillInfo[16]);
                SI.CanTargetSelf = byte.Parse(SkillInfo[17]) == 1 ? true : false;
                SI.Delay = ushort.Parse(SkillInfo[18]);
                SI.AutoAttack = byte.Parse(SkillInfo[19]) == 1 ? true : false;
                if (!Kernel.SkillInfos.ContainsKey(SI.ID))
                    Kernel.SkillInfos.TryAdd(SI.ID, new ConcurrentDictionary<byte, SkillInfo>());
                Kernel.SkillInfos[SI.ID].TryAdd(SI.Level, SI);
            }
        }
        public static void LoadSkills(SocketClient Client)
        {
            using (DataTable DT = Database.CharacterDB.GetDataTable("SELECT `ID`, `Level`, `Experience` FROM `skills` WHERE `OwnerUID`=" + Client.UniqueID))
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                    Client.Character.LearnSkill(Convert.ToUInt16(DT.Rows[i].ItemArray[0]), Convert.ToByte(DT.Rows[i].ItemArray[1]), Convert.ToUInt32(DT.Rows[i].ItemArray[2]));
            }
        }
    }
}