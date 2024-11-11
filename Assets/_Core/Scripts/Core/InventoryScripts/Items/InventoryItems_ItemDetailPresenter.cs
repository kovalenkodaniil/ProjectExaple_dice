using System;
using Core.Items;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_ItemDetailPresenter
    {
        public event Action<ItemConfig> OnItemSelected;
        
        private readonly InventoryItems_ItemDetailPanelView _view;
        private ItemConfig _currentItem;
        private ItemStorage _storage;

        public  InventoryItems_ItemDetailPresenter(InventoryItems_ItemDetailPanelView view, ItemStorage storage)
        {
            _view = view;
            _storage = storage;
        }

        public void Enable() => _view.OnItemSelected += SelectItem;

        public void Disable() => _view.OnItemSelected -= SelectItem;

        public void ShowSkillDetail(ItemConfig config)
        {
            _currentItem = config;

            UpdateSkillView();
        }

        public void UpdateSkillView()
        {
            _view.SetName(_currentItem.name);
            _view.SetDescription(_currentItem.description);
            _view.SetItemImage(_currentItem.icon);
            _view.SetActiveButton(!_storage.itemInBattle.Contains(_currentItem));
        }

        private void SelectItem()
        {
            OnItemSelected?.Invoke(_currentItem);
        }
    }
}