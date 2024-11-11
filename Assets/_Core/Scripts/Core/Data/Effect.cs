using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class Effect
    {
        [SerializeField] private EnumEffects _effectType;
        [SerializeField] private int _value;

        public EnumEffects EffectType => _effectType;
        public int Value => _value;
    }
}