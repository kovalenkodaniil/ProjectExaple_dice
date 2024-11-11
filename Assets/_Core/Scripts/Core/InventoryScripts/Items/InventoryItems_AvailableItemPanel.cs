using System;
using System.Collections.Generic;
using Core.Items;
using UnityEngine;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_AvailableItemPanel : MonoBehaviour
    {
        private ItemStorage _itemStorage;

        public event Action<ItemConfig> OnItemSelected;
        
        [SerializeField] private Transform _itemParent;
        [SerializeField] private InventoryItems_AvailableItemPreview _itemPreview;

        private List<InventoryItems_AvailableItemPresenter> _presenters;
        private InventoryItems_AvailableItemPresenter _currentPresenter;
        
        public void Initialize(ItemStorage itemStorage)
        {
            _itemStorage = itemStorage;
            
            CreateInventoryCells();
        }

        public void Enable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Enable();
                presenter.OnItemSelected += SelectItem;
            }

            _presenters[0].Select();
        }
        
        public void Disable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Disable();
                presenter.OnItemSelected -= SelectItem;
            }
        }

        public void SetBattleMark()
        {
            _currentPresenter?.UpdateView();
        }

        private void SelectItem(ItemConfig config, InventoryItems_AvailableItemPresenter presenter)
        {
            if (presenter == _currentPresenter) return;
            
            _currentPresenter?.Deselect();
            _currentPresenter = presenter;
            OnItemSelected?.Invoke(config);
        }

        private void CreateInventoryCells()
        {
            _presenters = new List<InventoryItems_AvailableItemPresenter>();
            
            foreach (var skill in _itemStorage.availableItem)
            {
                InventoryItems_AvailableItemPreview preview = Instantiate(_itemPreview, _itemParent);
                InventoryItems_AvailableItemPresenter presenter =
                    new InventoryItems_AvailableItemPresenter(skill, preview, _itemStorage);
                
                _presenters.Add(presenter);
            }
        }
    }
}