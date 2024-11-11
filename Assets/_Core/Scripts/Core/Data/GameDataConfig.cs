using System.Collections.Generic;
using Core.Data;
using UnityEngine;

namespace _Core.Scripts.Core.Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Game/Create new gameData", order = 0)]
    public class GameDataConfig : ScriptableObject
    {
        public List<DiceConfig> diceData;
        public PlayerStaticData playerData;
        public LocalizationData LocalizationData;
        public List<CombinationConfig> combinationConfigs;
    }
}