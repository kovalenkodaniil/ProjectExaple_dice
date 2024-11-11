using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Stat;
using Core.Features.Talents.Scripts;
using Managers;

namespace Core.Data
{
    public class EnemyData
    {
        public HealthComponent HealthComponent { get; }
        public ArmorComponent ArmorComponent { get; }
        public DamageComponent DamageComponent { get; }
        public List<EnemyIntention> Intentions;
        public List<EnemyTurn> PassiveSkills;

        public EnemyConfig config;

        public EnemyData(EnemyConfig enemyConfig, TalentManager talentManager)
        {
            HealthComponent = new HealthComponent(talentManager);
            ArmorComponent = new ArmorComponent();
            DamageComponent = new DamageComponent();
            
            HealthComponent.MaxValue = enemyConfig.health;
            
            Intentions = new List<EnemyIntention>();
            Intentions.AddRange(enemyConfig.intentions);
            
            PassiveSkills = new List<EnemyTurn>();

            config = enemyConfig;
        }
    }
}