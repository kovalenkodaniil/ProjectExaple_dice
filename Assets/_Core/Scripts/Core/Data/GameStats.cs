using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class GameStats
    {
        [SerializeField] private int battlesCounter;
        
        public int BattlesCounter
        {
            get => battlesCounter; 
            set => battlesCounter = value; 
        }

        public void Load(GameStats loadData)
        {
            if (loadData == null)
                return;

            battlesCounter = loadData.battlesCounter;
        }

        public void Reset()
        {
            battlesCounter = default;
        }
    }
}