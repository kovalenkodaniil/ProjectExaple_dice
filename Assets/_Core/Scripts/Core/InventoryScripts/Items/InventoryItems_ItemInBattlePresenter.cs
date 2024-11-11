using System;
using Core.Items;
using UnityEngine;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_ItemInBattlePresenter
    {
        private InBattlePreview _preview;
        private ItemConfig _itemConfig;

        public ItemConfig CurrentItem => _itemConfig;

        public InventoryItems_ItemInBattlePresenter(InBattlePreview preview, ItemConfig config)
        {
            _itemConfig = config;
            _preview = preview;
        }

        public void Initialize()
        {
            UpdatePreview();
        }

        public void Enable()
        {
            _preview.SetWaitEffect(true);
        }

        public void Disable()
        {
            _preview.SetWaitEffect(false);
        }

        public bool IsClickedOnPreview(Vector3 clickPosition)
        {
            var localClickPosition = _preview.RectTransform.InverseTransformPoint(clickPosition);
            
            return _preview.RectTransform.rect.Contains(localClickPosition);
        }

        public void ReplaceItem(ItemConfig config)
        {
            if (_itemConfig != null)
            {
                _preview.PLayHideAnimation(() =>
                {
                    UpdatePreview(false);
                });
            }
            else
            {
                UpdatePreview(false);
            }
            
            _itemConfig = config;
        }

        private void UpdatePreview(bool isFirstUpdate = true)
        {
            if (_itemConfig == null)
            {
                _preview.SetEmptyState();
                return;
            }

            _preview.SetIcon(_itemConfig.icon, isFirstUpdate);
        }
    }
}