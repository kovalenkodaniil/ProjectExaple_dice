using _Core.Scripts.Core.Battle.Dice;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.SkillScripts.SkillEffects
{
    public class SkillEffect : ScriptableObject
    {
        public void Apply(DiceTower diceTower, BattleUIPresenter uiPresenter)
        {
            OnApply(diceTower, uiPresenter);
        }

        protected virtual void OnApply(DiceTower diceTower, BattleUIPresenter uiPresenter)
        {
        }
    }
}