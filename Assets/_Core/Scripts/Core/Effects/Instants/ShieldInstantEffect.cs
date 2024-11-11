using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public class ShieldInstantEffect : InstantEffectBase
    {
        protected override void OnApply(CombatEntity entity, int power)
        {
            entity.AddShield(power);
        }
    }
}