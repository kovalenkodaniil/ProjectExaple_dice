using System.Collections;
using _Core.Scripts.Core.Battle.Dice;
using Managers;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.SkillScripts.SkillEffects
{
    [CreateAssetMenu(fileName = "Effect Reroll", menuName = "Skill/Skill Effect/Create Reroll")]
    public class SkillEffectReroll : SkillEffect
    {
        private int rollCount;
        private bool isWaiting;
        private Dice.Dice lastDice;
        private BattleUIPresenter uiPresenter;
        
        [SerializeField] private int diceInRoll;
        [SerializeField] private string description;

        protected override void OnApply(DiceTower diceTower, BattleUIPresenter uiPresenter)
        {
            rollCount = 0;
            this.uiPresenter = uiPresenter;
            
            SelectDice(diceTower);
            uiPresenter.SetSkillInfo(true, description);
        }
        
        public void SelectDice(DiceTower diceTower)
        {
            rollCount++;
            CoroutineManager.StartCoroutine(WaitSelect(diceTower));

            IEnumerator WaitSelect(DiceTower diceTower)
            {
                isWaiting = true;
                
                while (isWaiting)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (diceTower.diceChecker.IsDiceInPosition(Input.mousePosition, out Dice.Dice dice))
                        {
                            if (dice.IsDropped)
                            {
                                dice.IsDropped = false;
                                diceTower.RollSingleDice(dice);
                                isWaiting = false;
                                break;
                            }
                        }
                    }

                    yield return null;
                }
                
                yield return new WaitForFixedUpdate();

                if (rollCount < diceInRoll)
                    SelectDice(diceTower);
                else
                    uiPresenter.SetSkillInfo(false);
            }
        }
    }
}