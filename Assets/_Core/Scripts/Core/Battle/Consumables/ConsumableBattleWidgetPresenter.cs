using System;
using Core.Data.Consumable;
using Object = UnityEngine.Object;

namespace _Core.Scripts.Core.Battle.Consumables
{
    public class ConsumableBattleWidgetPresenter
    {
        public event Action<ConsumableBattleWidgetPresenter> OnClick;

        public ConsumableData data { get; private set; }
        private ConsumableBattleWidget view;

        public ConsumableBattleWidgetPresenter(ConsumableBattleWidget view, ConsumableData data)
        {
            this.view = view;
            this.data = data;

            view.Sprite = data.Sprite;
            view.AddListener(HandlerOnClick);
        }

        public void Destroy()
        {
            view.RemoveListener(HandlerOnClick);
            Object.Destroy(view.gameObject);
        }

        private void HandlerOnClick()
        {
            OnClick?.Invoke(this);
        }
    }
}