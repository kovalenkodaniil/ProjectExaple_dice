using System.Collections.Generic;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "DiceInfo", menuName = "Dice/Create dice")]
    public class DiceConfig : ScriptableObject
    {
        public string diceName;
        public string description;
        public string id;
        public string levelNumber;
        public int upgradeCost;
        public List<Edge> Edges;
        public List<DiceConfig> Upgrades;
        public Material DiceBorderMaterial;
    }
}