using System;
using System.Collections;
using _Core.Scripts.Core.Battle.Stat;
using UnityEngine;

namespace UI
{
    public class CoinStatCounter : StatCounter
    {
        private const int COUNTER_CHANGING_STEP = 5;
        private const float COUNTER_CHANGING_SPEED = 0.05f;
        private int _lastValue;

        public override void Initialize(IStatsComponent statsComponent)
        {
            base.Initialize(statsComponent);

            _lastValue = statsComponent.CurrentValue;
        }

        protected override void UpdateCounter(int value)
        {
            base.UpdateCounter(value);

            StartCoroutine(PlayUpdatingAnimation(_lastValue, value));
            
            _lastValue = value;
        }

        private IEnumerator PlayUpdatingAnimation(int oldValue, int currentValue)
        {
            int value = oldValue;

            while (true)
            {
                if (currentValue > oldValue)
                {
                    value += COUNTER_CHANGING_STEP;

                    if (value > currentValue)
                    {
                        value = currentValue;
                        _counter.text = $"{value}";
                        break;
                    }
                    
                    _counter.text = $"{value}";
                }
                else
                {
                    value -= COUNTER_CHANGING_STEP;
                    
                    if (value < currentValue)
                    {
                        value = currentValue;
                        _counter.text = $"{value}";
                        break;
                    }
                }

                _counter.text = $"{value}";
                
                yield return new WaitForSeconds(COUNTER_CHANGING_SPEED);
            }
        }
    }
}