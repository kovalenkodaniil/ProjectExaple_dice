using System;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "VFXStaticData", menuName = "Setting/Create new VFXSetting", order = 0)]
    public class VFXSetting : ScriptableObject
    {
        public List<BattleEffectSetting> battleEffects;
        public List<EnumEffects> emitOrder;
    }

    [Serializable]
    public class BattleEffectSetting
    {
        public EnumVFXEffect effectType;
        public UIParticle effect;
    }
}