using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "EnemyActionInfo", menuName = "Enemy/Create EnemyAction")]
    public class EnemyAction: ScriptableObject
    {
        public bool isCastAction;
        [HideIf("isCastAction")] public List<EnumEffects> effects;
        [HideIf("isCastAction")] public float powerModificator;
        public string description;
        public Sprite icon;
        [HideIf("isCastAction")] public int duration;
        [ShowIf("isCastAction")] public EnemyAction durationAbility;
    }
}