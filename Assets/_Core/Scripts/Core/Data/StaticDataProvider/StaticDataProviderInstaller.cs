using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using _Core.Scripts.Core.Battle.SkillScripts;
using Core.Data.Consumable;
using Core.Features.Talents.Data;
using Core.Items;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Object = UnityEngine.Object;

namespace Core.Data
{
    [DefaultExecutionOrder(-1)]
    public class StaticDataProviderInstaller : MonoBehaviour
    {
        [SerializeField] private ConsumableAsset consumables;
        [SerializeField] private EnemyAsset enemies;
        [SerializeField] private List<BattleData> battles;
        [SerializeField] private MaterialAsset materials;
        [SerializeField] private TalentAsset talents;
        [SerializeField] private List<SkillConfig> skills;
        [SerializeField] private List<EdgeColor> edgeColors;
        [SerializeField] private List<ItemConfig> itemConfigs;
        [SerializeField] private List<Effect> greynessEffects;

        private void Awake()
        {
            StaticDataProvider.Add(new ConsumableDataProvider(consumables));    
            StaticDataProvider.Add(new BattleDataProvider(battles, enemies));    
            StaticDataProvider.Add(new MaterialDataProvider(materials));    
            StaticDataProvider.Add(new TalentAssetProvider(talents));    
            StaticDataProvider.Add(new TalentAssetProvider(talents));    
            StaticDataProvider.Add(new SkillDataProvider(skills));    
            StaticDataProvider.Add(new EdgeColorDataProvider(edgeColors));
            StaticDataProvider.Add(new ItemDataProvider(itemConfigs));
            StaticDataProvider.Add(new GreynessStaticData(greynessEffects));
        }

        [Button] public void UpdateBattles()
        {
#if UNITY_EDITOR
            battles = FindAssets<BattleData>("Battle");
#endif
        }

#if UNITY_EDITOR
        private List<T> FindAssets<T>(string folderName) where T : Object
        {   
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { $"Assets/DataSO/{folderName}" });

            var list = new List<T>(50);

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (obj != null)
                {
                    list.Add(obj);
                }
            }

            return list;
        }
#endif
    }
}