using System;

namespace _Core.Scripts.Core.Battle.Stat
{
    public interface IStatsComponent
    {
        public Action<int> OnValueChanged { get; set; }

        public int CurrentValue { get; }
        public int MaxValue { get; set; }
        
        public void IncreaseMaxValue(int value = 1)
        {
            MaxValue += value;
        }
    }
}