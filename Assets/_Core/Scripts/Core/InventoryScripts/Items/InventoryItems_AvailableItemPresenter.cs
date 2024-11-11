using System;
using Core.Items;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_AvailableItemPresenter
    {
        public event Action<ItemConfig, InventoryItems_AvailableItemPresenter> OnItemSelected;

        private ItemStorage _storage;
        private InventoryItems_AvailableItemPreview _view;
        private ItemConfig _config;

        public InventoryItems_AvailableItemPresenter(ItemConfig config, InventoryItems_AvailableItemPreview view, ItemStorage storage)
        {
            _view = view;
            _config = config;
            _storage = storage;
        }

        public void Enable()
        {
            UpdateView();
            _view.OnPreviewClicked += SelectSkill;
        }

        public void Disable()
        {
            UpdateView();
            _view.OnPreviewClicked -= SelectSkill;
        }

        public void Deselect()
        {
            _view.HideSelectingEffect();
            UpdateViewVariant();
        }

        public void Select() => SelectSkill();

        public void UpdateView()
        {
            _view.Icon.sprite = _config.icon;

            UpdateViewVariant();
        }

        private void UpdateViewVariant()
        {
            if (_storage.itemInBattle.Contains(_config))
                _view.SetInBattleVariant();
            else
                _view.SetDefaultVariant();
        }

        private void SelectSkill()
        {
            SetSelectingView();
            OnItemSelected?.Invoke(_config, this);
        }

        private void SetSelectingView()
        {
            _view.SetSelectingVariant();
        }
    }
}