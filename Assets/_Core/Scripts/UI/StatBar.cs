using _Core.Scripts.Core.Battle.Stat;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatBar : StatCounter
    {
        [SerializeField] private Slider _bar;
        [SerializeField] protected StatBarAnimator _animator;

        private int _lastValue;
        
        public override void Initialize(IStatsComponent statsComponent)
        {
            base.Initialize(statsComponent);
            
            _bar.maxValue = _statsComponent.MaxValue;
            _bar.value = _statsComponent.CurrentValue;
        }

        protected override void UpdateCounter(int value)
        {
            base.UpdateCounter(value);
            
            switch (_statsComponent)
            {
                case HealthComponent:
                    PLayHealthAnimation(value);
                    break;
                
                case ManaComponent:
                    PLayManaAnimation(value);
                    break;
            }

            _lastValue = value;
        }

        private void PLayHealthAnimation(int value)
        {
            if (value < _lastValue)
                _animator.PlayDamageAnimation(value, _lastValue);
            else
                _animator.PlayHealAnimation(value, _lastValue);
        }
        
        private void PLayManaAnimation(int value)
        {
            if (value < _lastValue)
                _animator.PlayManaDecrease(value, _lastValue);
            else
                _animator.PlayManaIncrease(value, _lastValue);
        }
    }
}