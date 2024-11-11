using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public interface IInstantEffect
    {
        void Apply(CombatEntity entity, int power);
    }
}