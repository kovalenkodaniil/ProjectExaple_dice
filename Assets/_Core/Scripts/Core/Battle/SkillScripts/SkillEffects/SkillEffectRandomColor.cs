using System.Collections;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using Managers;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.SkillScripts.SkillEffects
{
    [CreateAssetMenu(fileName = "Effect Random Color", menuName = "Skill/Skill Effect/Create Random Color")]
    public class SkillEffectRandomColor : SkillEffect
    {
        private int rollCount;
        private bool isWaiting;
        private EdgeColorDataProvider provider;
        private BattleUIPresenter uiPresenter;
        
        [SerializeField] private int diceInRoll;
        [SerializeField] private string description;

        protected override void OnApply(DiceTower diceTower, BattleUIPresenter uiPresenter)
        {
            rollCount = 0;
            provider = StaticDataProvider.Get<EdgeColorDataProvider>();
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
                            dice.ChangeColor(provider.GetAnotherRandomColor(dice._data.TopEdgeColor));
                            isWaiting = false;
                            break;
                        }
                    }

                    yield return null;
                }
                
                yield return new WaitForSeconds(0.2f);

                if (rollCount < diceInRoll)
                    SelectDice(diceTower);
                else
                    uiPresenter.SetSkillInfo(false);
            }
        }
    }
}