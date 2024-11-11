using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public interface IBuff
    {
        bool Applyed { get; }
        int RemainingTurns { get; }
        void Apply(CombatEntity entity, int power, int turns, bool isPermanent);
        public void Remove();
        bool IsExpired();
        void OnTurnEnd();
    }
}