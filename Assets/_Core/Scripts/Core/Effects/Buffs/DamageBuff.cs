using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public class DamageBuff : BuffBase
    {
        protected override void OnApply(CombatEntity entity, int power)
        {
            entity.ApplyDamageBuff(power);
        }

        protected override void OnRemove()
        {
            entity.ApplyDamageBuff(-power);
        }
    }
}