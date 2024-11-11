using UnityEngine;

namespace Core.Items
{
    public enum ItemType
    {
        TrumpСard = 1,
        TheBronzeHorseshoe = 2
    }

    [CreateAssetMenu(fileName = "Item config", menuName = "Item/Create new Item", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        public string id;
        public string name;
        public Sprite icon;
        public string description;
        public ItemType type;
        public int value;
    }
}