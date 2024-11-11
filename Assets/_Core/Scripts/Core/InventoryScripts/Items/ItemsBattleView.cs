using System.Collections.Generic;
using Core.Items;
using UnityEngine;

namespace Core.InventoryScripts.Items
{
    public class ItemsBattleView : MonoBehaviour
    {
        private ItemStorage _storage;
        
        [SerializeField] private InBattlePreview _previewPrefab;
        [SerializeField] private Transform _itemParent;

        private List<InBattlePreview> _views;
        
        public void Initialize(ItemStorage storage)
        {
            _storage = storage;
            
            CreateItems();
        }

        public void OnDisable()
        {
            _views?.ForEach(view => Destroy(view.gameObject));
        }

        public void CreateItems()
        {
            _views = new List<InBattlePreview>();
            
            _storage.itemInBattle.ForEach(item =>
            {
                InBattlePreview preview = Instantiate(_previewPrefab, _itemParent);
                
                preview.SetIcon(item.icon);
                preview.EnableTooltip(item.description);
                
                _views.Add(preview);
            });
        }
    }
}