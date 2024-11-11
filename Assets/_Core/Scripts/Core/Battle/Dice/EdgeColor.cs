using UnityEngine;

namespace _Core.Scripts.Core.Battle.Dice
{
    [CreateAssetMenu(fileName = "EdgeColor", menuName = "Dice/Create new color")]
    public class EdgeColor : ScriptableObject
    {
        public EnumEdgeColor type;
        public Color color;
    }
    
    public enum EnumEdgeColor
    {
        Red = 1,
        Yellow = 2,
        Green = 3,
        Blue = 4,
        Black = 5,
        Empty = 6
    }
}