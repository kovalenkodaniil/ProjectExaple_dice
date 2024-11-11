using System.Collections;
using _Core.Scripts.Core.Battle.Dice;
using Managers;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.SkillScripts.SkillEffects
{
    [CreateAssetMenu(fileName = "Target Color", menuName = "Skill/Skill Effect/Create Target Color")]
    public class SkillEffectTargetColor : SkillEffect
    {
        private BattleUIPresenter uiPresenter;
        
        [SerializeField] private string description;
        
        protected override void OnApply(DiceTower diceTower, BattleUIPresenter uiPresenter)
        {
            this.uiPresenter = uiPresenter;
            
            SelectDice(diceTower);
            uiPresenter.SetSkillInfo(true, description);
        }
        
        public void SelectDice(DiceTower diceTower)
        {
            CoroutineManager.StartCoroutine(WaitSelect(diceTower));

            IEnumerator WaitSelect(DiceTower diceTower)
            {
                while (true)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (diceTower.diceChecker.IsDiceInPosition(Input.mousePosition, out Dice.Dice dice))
                        {
                            diceTower.ShowColorPopup(dice);
                            uiPresenter.SetSkillInfo(false);
                            break;
                        }
                    }

                    yield return null;
                }
            }
        }
    }
}