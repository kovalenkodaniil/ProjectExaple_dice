using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public class HealthBuffEffect: InstantEffectBase
    {
        protected override void OnApply(CombatEntity entity, int power)
        {
            entity.IncreaseBaseHealth(power);
        }
    }
}