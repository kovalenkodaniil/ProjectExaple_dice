using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerScripts
{
    [Serializable]
    public class PlayerModel
    {
        public event Action OnExpChanged;
        
        [SerializeField] private int expirience;
        
        public int Expirience => expirience;

        public bool CanSpend(int value)
        {
            return expirience >= value;
        }

        public void AddExpirience(int value)
        {
            expirience += value;
            OnExpChanged?.Invoke();
        }

        public void SpendExpirience(int value)
        {
            expirience -= value;
            OnExpChanged?.Invoke();
        }
    }
}