using System;
using Core.Features.Talents.Scripts;

namespace _Core.Scripts.Core.Battle.Stat
{
    public class ManaComponent : IStatsComponent
    {
        public int mana;
        private int _maxMana;
        private TalentManager talentManager;

        public int MaxValue
        {
            get => CalculateMaxMana();
            set
            {
                _maxMana = value;
                mana = value;
            }
        }

        public Action<int> OnValueChanged { get; set; }

        public ManaComponent(TalentManager talentManager, int startMana = 0)
        {
            this.talentManager = talentManager;
            mana = startMana;
        }
        
        public int CurrentValue => mana;

        public bool IsFull => mana == MaxValue;

        public void AddMana(int value)
        {
            if (value <=0) return;

            mana += value;

            if (mana >= MaxValue)
                mana = MaxValue;
            
            OnValueChanged?.Invoke(mana);
        }

        public bool TrySpend(int value)
        {
            if (value > mana) return false;

            mana -= value;
            OnValueChanged?.Invoke(mana);

            return true;
        }

        private int CalculateMaxMana()
        {
            return _maxMana + talentManager.GetMaxMana();
        }
    }
}