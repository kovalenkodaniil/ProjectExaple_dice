using System.Collections;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using Core.Data.Consumable;
using Managers;
using PlayerScripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Core.Scripts.Core.Battle.Consumables
{
    public class ConsumableBattlePresenter
    {
        private bool isLocked;
        private List<ConsumableBattleWidgetPresenter> presenters = new(3);
        private ConsumablesBattleView view;
        private Player player;
        private TurnManager turnManager;
        private DiceTower diceTower;
        private BattleUIPresenter battleUIPresenter;

        public ConsumableBattlePresenter(ConsumablesBattleView view, Player player, TurnManager turnManager, DiceTower diceTower, BattleUIPresenter battleUIPresenter)
        {
            this.turnManager = turnManager;
            this.player = player;
            this.view = view;
            this.diceTower = diceTower;
            this.battleUIPresenter = battleUIPresenter;

            this.turnManager.OnStartTurn += HandlerOnStartTurn;
        }

        public void SetLock(bool value)
        {
            isLocked = value;
        }

        public void Destroy()
        {
            DestroyAll();
            this.turnManager.OnStartTurn -= HandlerOnStartTurn;
        }

        public void SetConsumables(List<ConsumableData> list)
        {
            DestroyAll();

            if (list.Count == 0)
            {
                view.SetActive(false);
                return;
            }
            
            var prefab = view.WidgetTemplate;

            foreach (var consData in list)
            {
                if (consData.ConsumableType == EnumConsumable.Palette) continue;
                
                var viewInstance = Object.Instantiate(prefab, view.Content);
                viewInstance.gameObject.SetActive(true);
                var pres = new ConsumableBattleWidgetPresenter(viewInstance, consData);
                presenters.Add(pres);
                pres.OnClick += HandlerOnClickWidget;
            }
        }

        private void HandlerOnStartTurn()
        {
            isLocked = false;
        }

        private void HandlerOnClickWidget(ConsumableBattleWidgetPresenter presenter)
        {
            if(isLocked)
                return;
            
            isLocked = true;
            bool isUsed = false;
            var data = presenter.data;
            
            foreach (var effect in data.Effects)
            {
                switch (effect.EffectType)
                {
                    case EnumEffects.Heal:
                        if (!player.HealthComponent.IsFull)
                        {
                            player.HealthComponent.Heal(effect.Value);
                            isUsed = true;
                        }
                        break;
                    
                    case EnumEffects.Mana:
                        if (!player.ManaComponent.IsFull)
                        {
                            player.ManaComponent.AddMana(effect.Value);
                            isUsed = true;
                        }
                        break;
                    
                    case EnumEffects.RandomColor:
                        SelectDice(diceTower);
                        isUsed = true;
                        break;
                }
            }

            if (isUsed)
            {
                presenters.Remove(presenter);
                presenter.Destroy();
                player.consumableStorage.TrySpend(data.ConsumableType);
            }
        }

        private void DestroyAll()
        {
            if (presenters.Count > 0)
            {
                foreach (var pres in presenters)
                    pres.Destroy();
                
                presenters.Clear();
            }
        }
        
        public void SelectDice(DiceTower diceTower)
        {
            var provider = StaticDataProvider.Get<EdgeColorDataProvider>();
            battleUIPresenter.SetSkillInfo(true, "Выбери кубик");
            CoroutineManager.StartCoroutine(WaitSelect(diceTower));

            IEnumerator WaitSelect(DiceTower diceTower)
            {
                while (true)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (diceTower.diceChecker.IsDiceInPosition(Input.mousePosition, out Dice.Dice dice))
                        {
                            dice.ChangeColor(provider.GetAnotherRandomColor(dice._data.TopEdgeColor));
                            battleUIPresenter.SetSkillInfo(false);
                            break;
                        }
                    }

                    yield return null;
                }
                
                yield return null;
            }
        }
    }
}