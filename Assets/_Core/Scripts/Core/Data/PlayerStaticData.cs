using System.Collections.Generic;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "PlayerStaticData", menuName = "Game Data/Create Player data", order = 0)]
    public class PlayerStaticData : ScriptableObject
    {
        public int maxHealth;
        public int startMana;
        public int startFixation;
        public int startExpirience;
        public List<DiceConfig> startDices;
        public List<DiceData> startAvailableDice;
    }
}