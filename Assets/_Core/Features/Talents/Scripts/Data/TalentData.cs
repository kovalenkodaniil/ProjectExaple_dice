using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Drawer.ListDisplay;

namespace Core.Features.Talents.Data
{
    public enum EnumTalentEffect
    {
        none,
        health,
        mana,
        fixations,
        magicArrow,
        magicShield
    }
    
    [CreateAssetMenu(menuName = "Data/Talents/Talent")]
    public class TalentData : ScriptableObject
    {
        [Serializable]
        public class LevelData
        {
            public int exp;
            public EnumTalentEffect effect;
            public int amount;
        }

        [SerializeField] private string id;
        [SerializeField] private string name;
        [ListDisplay(ListDisplayAttribute.DisplayMode.Inline)] public List<LevelData> levels;
        public List<TalentData> prerequisites;
        public Sprite sprIcon;

        public string Id => id;
        public int MaxLvl => levels.Count;
        public string Name => name;
    }
}