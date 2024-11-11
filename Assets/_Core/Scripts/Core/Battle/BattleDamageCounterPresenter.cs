using _Core.Scripts.Core.Battle.Stat;
using TMPro;
using UnityEngine;

namespace _Core.Scripts.Core.Battle
{
    public class BattleDamageCounterPresenter : MonoBehaviour
    {
        private TurnManager turnManager;
    
        private IStatsComponent component;
    
        [SerializeField] private TMP_Text lbCounter;
        [SerializeField] private TMP_Text lbName;
    
        private string Counter { set => lbCounter.text = value; }
    
        public void Construct(IStatsComponent component)
        {
            this.component = component;
        }

        public void Enable()
        {
            component.OnValueChanged += OnValueChanged;
            OnValueChanged(component.CurrentValue);
        }

        public void Disabe()
        {
            component.OnValueChanged -= OnValueChanged;
        }

        private void OnValueChanged(int curr)
        {
            lbCounter.text = curr.ToString();
            gameObject.SetActive(curr > 0);
        
        }
    }
}