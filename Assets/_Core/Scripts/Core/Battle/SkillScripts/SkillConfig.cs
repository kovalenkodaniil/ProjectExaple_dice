using _Core.Scripts.Core.Battle.SkillScripts.SkillEffects;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.SkillScripts
{
    public enum EnumActivationPhase
    {
        BeforeRoll = 1,
        AfterRoll = 2
    }

    [CreateAssetMenu(fileName = "Skill Config", menuName = "Skill/Create new skill")]
    public class SkillConfig : ScriptableObject
    {
        public string id;
        public Sprite icon;
        public string spellName;
        public string description;
        public string shortDescription;
        public int cooldownMax;
        public int manaCost;
        public EnumActivationPhase activationPhase;
        public SkillEffect effect;
        
        public Color SelectedColor => new Color(0.454902f, 0.5686275f, 0.854902f);
        public Color InBattlePreviewColor => new Color(0.7411765f, 0.7921569f, 0.8941177f);
    }
}