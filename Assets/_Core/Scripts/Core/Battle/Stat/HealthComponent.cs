using System;
using Core.Features.Talents.Scripts;

namespace _Core.Scripts.Core.Battle.Stat
{
    public class HealthComponent: IStatsComponent
    {
        public Action<int> OnValueChanged { get; set; }
        public Action OnDied;
        
        public int Health { get; private set; }
        private int _maxHealth;
        
        private TalentManager talentManager;
        
        public HealthComponent(TalentManager talentManager)
        {
            this.talentManager = talentManager;
        }
        
        public int CurrentValue => Health;
        public int MaxValue
        {
            get => CalculateMaxValue();
            set
            {
                _maxHealth = value;
                Health = value;
            }
        }
        public bool IsFull => Health == MaxValue;
        

        public void Reset()
        {
            Health = MaxValue;
        }

        public void TakeDamage(int value)
        {
            if (value <= 0) return;

            Health -= value;

            if (Health <= 0)
                Health = 0;

            if (Health == 0)
                OnDied?.Invoke();
            
            OnValueChanged?.Invoke(Health);
        }
        
        public void TakeNoLethalDamage(int value)
        {
            if (value >= Health)
                Health = 1;
            else
                Health -= value;
            
            OnValueChanged?.Invoke(Health);
        }

        public void Heal(int value)
        {
            if (value <= 0) return;

            Health += value;
            
            if (Health > MaxValue)
                Health = MaxValue;
            
            OnValueChanged?.Invoke(Health);
        }

        private int CalculateMaxValue()
        {
            return _maxHealth + talentManager.GetMaxHealth();
        }
    }
}