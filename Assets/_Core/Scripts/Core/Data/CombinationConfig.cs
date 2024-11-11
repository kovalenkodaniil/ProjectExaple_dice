using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Core.Features.Talents.Data;
using Core.Features.Talents.Scripts;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Data/Battle/Combo")]
    public class CombinationConfig : ScriptableObject
    {
        public string id;
        public List<EdgeColor> comboSequence;
        public List<Effect> effects;
        public EnumTalentEffect requirement;

        public bool IsUnlocked(TalentManager talentManager)
        {
            switch (requirement)
            {
                case EnumTalentEffect.none:
                    return true;
                case EnumTalentEffect.magicArrow:
                    return talentManager.IsMagicArrowOpened();
                case EnumTalentEffect.magicShield:
                    return talentManager.IsMagicShieldOpened();
            }

            return false;
        }

        public bool TryActivateCombo(List<EnumEdgeColor> diceEdges, ref int activationsCount)
        {
            int coincidencesCount = 0;
            
            comboSequence.ForEach(color =>
            {
                if (diceEdges.Contains(color.type))
                {
                    diceEdges.Remove(color.type);
                    coincidencesCount++;
                }
            });

            if (coincidencesCount == comboSequence.Count)
            {
                activationsCount++;
                TryActivateCombo(diceEdges, ref activationsCount);
            }

            return activationsCount > 0;
        }
    }
}