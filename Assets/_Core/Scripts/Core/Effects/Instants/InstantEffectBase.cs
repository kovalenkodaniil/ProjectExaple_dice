using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public abstract class InstantEffectBase : Effect, IInstantEffect
    {
        public void Apply(CombatEntity entity, int power)
        {
            OnApply(entity, power);
        }

        protected abstract void OnApply(CombatEntity entity, int power);
    }
}