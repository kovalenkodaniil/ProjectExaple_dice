using System;

namespace _Core.Scripts.Core.Battle.Stat
{
    public class KarmaComponent : IStatsComponent
    {
        public int Value { get; private set; }

        public int MaxValue { get; set; }
        public Action<int> OnValueChanged { get; set; }
        
        public int CurrentValue => Value;

        public void RestoreValue(int value)
        {
            if (value <= 0) return;
            
            Value = value;
        }
        
        public void AddKarma(int value)
        {
            Value += value;
            
            OnValueChanged?.Invoke(value);
        }
    }
}