using System;
using Core.Features.Talents.Scripts;

namespace _Core.Scripts.Core.Battle.Stat
{
    public class FixationComponent: IStatsComponent
    {
        private int _startFixation;
        private TalentManager talents;

        public int MaxValue 
        { 
            get => CalculateMaxFixations();
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                
                _startFixation = value;
                Fixation = value;
            } 
        }
        public int Fixation { get; private set; }

        public Action<int> OnValueChanged { get; set; }

        public int CurrentValue => Fixation;

        public FixationComponent(TalentManager talents)
        {
            this.talents = talents;
        }

        public int CalculateMaxFixations() => _startFixation + talents.GetMaxFixations();
    }
}