using System.Collections.Generic;
using Core.InventoryScripts;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Core.Data.Consumable
{
    [CreateAssetMenu(menuName = "Data/Assets/ConsumableAsset")]
    public class ConsumableAsset : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private int maxConsumables = 3;
        
        [Header("Consumables")]
        [SerializeField] private List<ConsumableData> consumables;

        [Header("Prefabs")] 
        [SerializeField] private InBattlePreview inBattlePreviewPrefab;
        [SerializeField] private InventoryPreview inventoryPreviewPrefab;

        public int MaxConsumables => maxConsumables;
        public List<ConsumableData> Consumables => consumables;
        public InBattlePreview InBattlePreviewPrefab => inBattlePreviewPrefab;
        public InventoryPreview InventoryPreviewPrefab => inventoryPreviewPrefab;

        [Button]
        public void LoadAllConsumables()
        {
#if UNITY_EDITOR
            consumables = StaticDataProvider.FindAssets<ConsumableData>("Consumable");
            AssetDatabase.SaveAssets();
#endif
        }

        public void SetConsumables(List<ConsumableData> list)
        {
            consumables = list;
        }
    }
}