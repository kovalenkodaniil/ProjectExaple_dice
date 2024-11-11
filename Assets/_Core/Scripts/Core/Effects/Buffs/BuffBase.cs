using _Core.Scripts.Core.Battle.Enemies;

namespace Core.Effects
{
    public abstract class BuffBase : Effect, IBuff
    {
        protected int power;
        protected CombatEntity entity;
        
        private bool applyed;
        private int remainingTurns;
        private bool isRemoved;
        private bool isPermanent;
        
        public int RemainingTurns { get => remainingTurns; }
        public bool Applyed { get => applyed; }

        public void Apply(CombatEntity entity, int power, int turns, bool isPermanent = false)
        {
            this.power = power;
            this.entity = entity;
            applyed = true;
            remainingTurns = turns;
            this.isPermanent = isPermanent;
            OnApply(entity, power);
        }

        public void Remove()
        {
            isRemoved = true;
            OnRemove();
        }

        public bool IsExpired()
        {
            return remainingTurns <= 0;
        }

        public void OnTurnEnd()
        {
            if (!isRemoved && !isPermanent && --remainingTurns == 0)
                Remove();
        }

        protected abstract void OnApply(CombatEntity entity, int power);

        protected abstract void OnRemove();
    }
}