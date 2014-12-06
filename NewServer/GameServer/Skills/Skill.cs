using System;
namespace GameServer.Skills
{
    public class Skill
    {
        public uint Experience;
        public Skills.SkillInfo SkillInfo;
        public Skill(Skills.SkillInfo SI) { this.SkillInfo = SI; }
    }
}
