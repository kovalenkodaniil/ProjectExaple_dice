using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public class StunIntentionInstantEffect : InstantEffectBase
    {
        protected override void OnApply(CombatEntity entity, int power)
        {
            if(entity is EnemyCombatEntity ece)
                ece.StunIntention(power);
        }
    }
}