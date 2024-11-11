using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data.Consumable;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace _Core.Scripts.Core.Battle.Consumables
{
    public class ConsumablesBattleView : MonoBehaviour
    {
        [Inject] private DiceTower diceTower;
        [Inject] private BattleUIPresenter battleUIPresenter;
        
        private ConsumableBattlePresenter presenter;

        [SerializeField] private RectTransform content;
        [SerializeField] private ConsumableBattleWidget widgetTemplate;

        public RectTransform Content => content;
        public ConsumableBattleWidget WidgetTemplate => widgetTemplate;

        private void Start()
        {
            widgetTemplate.gameObject.SetActive(false);
        }

        public void Construct(Player player, TurnManager turnManager, List<ConsumableData> list)
        {
            presenter = new(this, player, turnManager, diceTower, battleUIPresenter);
            SetConsumables(list);
        }
        
        public void SetConsumables(List<ConsumableData> list) => presenter.SetConsumables(list);

        public void SetLock(bool value) => presenter.SetLock(value);

        public void SetActive(bool value) => gameObject.SetActive(value);

        private void OnDestroy() => presenter.Destroy();
    }
}