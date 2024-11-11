using Core.Data.Consumable;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class InventoryConsumable_InBattlePanelPresenter
    {
        private InBattlePreview view;
        public ConsumableData data { get; private set; }

        public InventoryConsumable_InBattlePanelPresenter(InBattlePreview view, ConsumableData config)
        {
            this.data = config;
            this.view = view;
            SetData(data);
        }
        
        public void SetData(ConsumableData newData)
        {
            this.data = newData;
            UpdateView();
        }

        public void SetPlayWaitEffect(bool value)
        {
            view.SetWaitEffect(value);
        }

        public void UpdateView()
        {
            if (data == null)
            {
                view.SetEmptyState();
            }
            else
            {
                view.SetIcon(data.IconInPanel, data.ShadowSprite);   
            }
        }

        public void Destroy()
        {
            Object.Destroy(view.gameObject);
        }
    }
}