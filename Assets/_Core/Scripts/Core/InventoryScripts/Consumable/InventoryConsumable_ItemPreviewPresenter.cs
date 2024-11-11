using System;
using System.Linq;
using Core.Data.Consumable;
using Core.Localization;
using Managers;
using PlayerScripts;
using VContainer;

namespace Core.InventoryScripts
{
    public class InventoryConsumable_ItemPreviewPresenter
    {
        [Inject] private Player _player;
        [Inject] private Managers.Localization localization;
        [Inject] private FontSettings _fontSettings;
        
        public event Action<InventoryConsumable_ItemPreviewPresenter> OnPreviewClicked;
        public event Action<ConsumableData> OnSelectedForBattle;

        public ConsumableData data { get; private set; }
        private bool isReserved;
        private InventoryConsumable_ItemDetailPanel itemDetailPanel;
        private InventoryPreview view;

        public InventoryConsumable_ItemPreviewPresenter(bool isReserved, InventoryConsumable_ItemDetailPanel itemDetailPanel, InventoryPreview view,
            ConsumableData data)
        {
            this.itemDetailPanel = itemDetailPanel;
            this.isReserved = isReserved;
            this.view = view;
            this.data = data;
        }

        public void Enable()
        {
            UpdatePreview();
            ShowItemDetail();
            SetInventoryPreviewLocalization();

            view.OnSpellPreviewClicked += HandlerItemViewClicked;
            view.OnPreviewDragStarted += TryDragSpell;
        }

        public void Disable()
        {
            view.OnSpellPreviewClicked -= HandlerItemViewClicked;
            view.OnPreviewDragStarted -= TryDragSpell;
            itemDetailPanel.OnSelected -= InvokeOnSelectedForBattle;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(view.gameObject);
        }

        public void SelectFirst()
        {
            ShowItemDetail();
            SetSelectingView();
        }

        public void HandlerItemViewClicked()
        {
            itemDetailPanel.PlayHideContent(ShowItemDetail);
            SetSelectingView();
        }

        public void TryDragSpell()
        {
            HandlerItemViewClicked();
            
            itemDetailPanel.OnClickSelected();
        }

        private void SetSelectingView()
        {
            view.SetIcon(data.Sprite);
            view.SetSelectingVariant();
            
            OnPreviewClicked?.Invoke(this);
            
            itemDetailPanel.OnSelected -= InvokeOnSelectedForBattle;
            itemDetailPanel.OnSelected += InvokeOnSelectedForBattle;
        }

        public void SetReserved(bool value)
        {
            isReserved = value;
        }

        public void OffPreview()
        {
            UpdatePreview();
            view.HideSelectingEffect();

            itemDetailPanel.OnSelected -= InvokeOnSelectedForBattle;
        }

        private void ShowItemDetail()
        {
            itemDetailPanel.IconConsumable = data.Sprite;
            itemDetailPanel.Name = Managers.Localization.Translate(data.NameId);
            itemDetailPanel.EffectName = Managers.Localization.Translate(data.EffectId);
            itemDetailPanel.Description = Managers.Localization.Translate(data.DescriptionId);

            var power = data.Effects.First().Value;
            itemDetailPanel.EffectPower = power > 9 ? power.ToString() : $"0{power}";
            
            bool isInteractable = !isReserved && _player.consumableStorage.CountAll() > _player.inBattleConsumablesService.InBattleCount;
            itemDetailPanel.SetInteractableButton(isInteractable);
            
            itemDetailPanel.PlayAppearanceContent();
        }

        public void UpdatePreview()
        {
            if (isReserved)
            {
                view.SetIcon(data.IconInPanel);
                view.SetInBattleVariant();
                view.SetDragable(false);
            }
            else
            {
                view.SetIcon(data.Sprite);
                view.SetDefaultVariant();
                view.SetDragable(true);
            }
        }

        private void SetInventoryPreviewLocalization()
        {
            view.SetLocalization(localization, _fontSettings);
        }

        private void InvokeOnSelectedForBattle()
        {
            OnSelectedForBattle?.Invoke(data);
        }
    }
}