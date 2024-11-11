using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Enemies;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Data/Battle/Enemy")]
    public class EnemyConfig : ScriptableObject
    {
        public const string enemy_inquisitor     = nameof(enemy_inquisitor);
        public const string enemy_guardsman      = nameof(enemy_guardsman);
        public const string enemy_highinquisitor = nameof(enemy_highinquisitor);
        
        public string id;
        public int health;
        public Sprite sprite;
        public EnemyBase prefab;
        public List<EnemyIntention> intentions;
    }

    #region Editor

#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyConfig))]
    public class EnemyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EnemyConfig enemy = (EnemyConfig)target;
            Rect rect = EditorGUILayout.GetControlRect(false, 120);
            enemy.sprite = (Sprite)EditorGUI.ObjectField(rect, "", enemy.sprite, typeof(Sprite), false);
            base.OnInspectorGUI();
        }
    }
#endif

    #endregion
}


