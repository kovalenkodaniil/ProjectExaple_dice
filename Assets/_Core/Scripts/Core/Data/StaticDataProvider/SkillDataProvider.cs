using System.Collections.Generic;
using _Core.Scripts.Core.Battle.SkillScripts;

namespace Core.Data
{
    public class SkillDataProvider: IStaticDataProvider
    {
        public List<SkillConfig> skills;

        public SkillDataProvider(List<SkillConfig> skills)
        {
            this.skills = skills;
        }
    }
}