using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public class HealInstantEffect : InstantEffectBase
    {
        protected override void OnApply(CombatEntity entity, int power)
        {
            entity.Heal(power);
        }
    }
}