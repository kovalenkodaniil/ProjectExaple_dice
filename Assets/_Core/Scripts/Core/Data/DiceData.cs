using System;

namespace Core.Data
{
    [Serializable]
    public class DiceData
    {
        public event Action OnUpgraded;
        public event Action OnAddedInBattle;
        public event Action OnRemovedFromBattle;
        
        public DiceConfig config;
        public int quantity;

        public DiceData(DiceConfig config, int quantity)
        {
            this.config = config;
            this.quantity = quantity;
        }

        public void Upgrade(DiceConfig newConfig)
        {
            config = newConfig;
            
            OnUpgraded?.Invoke();
        }

        public void AddInBattle() => OnAddedInBattle?.Invoke();

        public void RemoveFromBattle() => OnRemovedFromBattle?.Invoke();
    }
}