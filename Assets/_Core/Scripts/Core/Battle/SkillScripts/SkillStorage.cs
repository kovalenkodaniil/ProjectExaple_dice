using System.Collections.Generic;
using Core.Data;

namespace _Core.Scripts.Core.Battle.SkillScripts
{
    public class SkillStorage
    {
        public List<SkillConfig> skillInBattle;
        public List<SkillConfig> availableSkills;

        public SkillStorage()
        {
            skillInBattle = new List<SkillConfig>();
            availableSkills = new List<SkillConfig>();
            
            availableSkills.AddRange(StaticDataProvider.Get<SkillDataProvider>().skills);
            skillInBattle.AddRange(StaticDataProvider.Get<SkillDataProvider>().skills);
        }

        public void ReplaceBattleSkill(SkillConfig oldSkill, SkillConfig newSkill)
        {
            skillInBattle.Remove(oldSkill);
            skillInBattle.Add(newSkill);
        }
    }
}