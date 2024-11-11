using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Data/Assets/EnemyAsset")]
    public class EnemyAsset : ScriptableObject
    {   
        public List<EnemyConfig> enemies;
        
        [Button]
        public void LoadAll()
        {
#if UNITY_EDITOR
            enemies = StaticDataProvider.FindAssets<EnemyConfig>("Enemies");
            AssetDatabase.SaveAssets();
#endif
        }
    }
}