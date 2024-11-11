using System.Collections.Generic;
using Core.Items;
using Managers;
using UnityEngine;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_ItemInBattlePanel : MonoBehaviour
    {
        private ItemStorage _storage;
        
        [SerializeField] private Transform _skillInBattleParent;
        [SerializeField] private InBattlePreview inBattlePreviewPrefab;

        private List<InventoryItems_ItemInBattlePresenter> _presenters;
        
        public void Initialize(ItemStorage storage)
        {
            _storage = storage;

            CreatePreview();
        }

        public void Enable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Initialize();
            }
        }

        public void Disable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Disable();
            }
        }

        public bool IsClickedOnPanel(Vector3 clickPosition, out InventoryItems_ItemInBattlePresenter selectedPresenter)
        {
            var worlClickPosition = GlobalCamera.Camera.ScreenToWorldPoint(clickPosition);
            
            foreach (var presenter in _presenters)
            {
                if (presenter.IsClickedOnPreview(worlClickPosition))
                {
                    selectedPresenter = presenter;
                    return true;
                }
            }
            
            selectedPresenter = null;
            return false;
        }

        public void ShowWaitingEffect(bool isShowing = true)
        {
            foreach (var presenter in _presenters)
            {
                if (isShowing)
                    presenter.Enable();
                else
                    presenter.Disable();
            }
        }

        private void CreatePreview()
        {
            _presenters = new List<InventoryItems_ItemInBattlePresenter>();
            
            foreach (var skill in _storage.itemInBattle)
            {
                InBattlePreview preview = Instantiate(inBattlePreviewPrefab, _skillInBattleParent);

                InventoryItems_ItemInBattlePresenter presenter = new InventoryItems_ItemInBattlePresenter(preview, skill);
                
                _presenters.Add(presenter);
            }
        }
    }
}