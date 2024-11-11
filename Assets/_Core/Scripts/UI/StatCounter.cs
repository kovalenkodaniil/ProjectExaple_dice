using _Core.Scripts.Core.Battle.Stat;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatCounter : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _counter;

        protected IStatsComponent _statsComponent;

        private void OnDisable()
        {
            if (_statsComponent == null) return;
            
            _statsComponent.OnValueChanged -= UpdateCounter;
        }

        protected virtual void UpdateCounter(int value)
        {
            _counter.text = $"{value}/{_statsComponent.MaxValue}";
        }

        public virtual void Initialize(IStatsComponent statsComponent)
        {
            _statsComponent = statsComponent;
            statsComponent.OnValueChanged += UpdateCounter;
            UpdateCounter(_statsComponent.CurrentValue);
        }
    }
}