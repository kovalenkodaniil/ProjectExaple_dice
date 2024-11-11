using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Scripts.Core.Data;
using Core.Data;
using Core.Data.Consumable;
using Core.Localization;
using Core.PreBattle;
using DG.Tweening;
using Localization;
using Managers;
using PlayerScripts;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using VContainer;

namespace Core.InventoryScripts
{
    public class InventoryConsumable_InventoryPanel : MonoBehaviour, ITab
    {
        #region Dependencies

        [Inject] private IObjectResolver _objectResolver;
        [Inject] private GameDataConfig _gameStaticData;
        [Inject] private Managers.Localization localization;

        #endregion
        
        public event Action OnOpen;
        public event Action<ITab> OnClose;

        private Player player;
        private ConsumableDataProvider provider;
        private ConsumableData currentSelectedItemData;
        private InventoryConsumable_ItemPreviewPresenter currentItem;
        private List<InventoryConsumable_ItemPreviewPresenter> inventoryPresenters = new(10);
        private List<InventoryConsumable_InBattlePanelPresenter> inBattlePresenters = new(10);
        private List<InventoryPreview> emptyItems = new(8);
        
        [SerializeField] private InventoryConsumable_ItemDetailPanel itemDetailWindow;
        [SerializeField] private InventoryBattlePanel _battlePanel;
        
        [Header("Parents")]
        [SerializeField] private Transform contentInventory;
        [SerializeField] private Transform contentInBattle;
        
        [Header("Components")]
        [SerializeField] private GameObject container;
        [SerializeField] private CanvasGroup canvasGroupViewInventory;
        [SerializeField] private CanvasGroup canvasGroupInBattle;
        [SerializeField] private CanvasGroup canvasGroupInventoryPanel;

        public void Initialize(Player player)
        {
            this.player = player;
            provider = StaticDataProvider.Get<ConsumableDataProvider>();
        }

        public void Open()
        {
            currentItem = null; // очищаем т.к. HandlerConsumSelected остается удаленный экземпляр
            
            if(inventoryPresenters.Count == 0)
                CreateConsumables();
            else
            {
                CreateEmptySells(8 - inventoryPresenters.Count);
            }

            InventoryPopup.OnStaticClose += OnClosedPopup;
            
            inventoryPresenters.ForEach(presenter =>
            {
                presenter.Enable();
                presenter.OnPreviewClicked += HandlerConsumSelected;
                presenter.OnSelectedForBattle += HandlerSelectItem;
            });
            
            foreach (var presenter in inBattlePresenters)
                presenter.UpdateView();

            if (inventoryPresenters.Count > 0)
            {
                inventoryPresenters[0].SelectFirst();
                canvasGroupInBattle.gameObject.SetActive(true);
                itemDetailWindow.SetActive(true);
            }
            else
            {
                canvasGroupInBattle.gameObject.SetActive(false);
                itemDetailWindow.SetActive(false);
            }
            
            container.SetActive(true);

            PlayOpenAnimation();
        }

        private void OnClosedPopup()
        {
            foreach (var pres in inventoryPresenters)
            {
                pres.Disable();
                pres.Destroy();
            }

            foreach (var pres in inBattlePresenters)
                pres.Destroy();

            inventoryPresenters.Clear();
            inBattlePresenters.Clear();
        }

        public void Close()
        {
            InventoryPopup.OnStaticClose -= OnClosedPopup;
            
            itemDetailWindow.SetActive(false);
            
            inventoryPresenters.ForEach(presenter =>
            {
                presenter.Disable();
                
                presenter.OnPreviewClicked -= HandlerConsumSelected;
                presenter.OnSelectedForBattle -= HandlerSelectItem;
            });

            PlayCloseAnimation(() =>
            {
                container.SetActive(false);
                OnClose?.Invoke(this);
            });
            
            foreach (var view in emptyItems)
            {
                Destroy(view.gameObject);
            }
            
            emptyItems.Clear();
        }
        
        public void PlayOpenAnimation()
        {
            canvasGroupViewInventory.DOFade(1, 0.4f);
            canvasGroupInventoryPanel.DOFade(1, 0.4f);
            canvasGroupInBattle.DOFade(1, 0.4f);
        }

        public void PlayCloseAnimation(Action callback)
        {
            canvasGroupViewInventory.DOFade(0, 0.4f);
            canvasGroupInventoryPanel.DOFade(0, 0.4f);
            canvasGroupInBattle.DOFade(0, 0.4f).OnComplete(() => callback?.Invoke());
        }

        private void CreateConsumables()
        {
            inventoryPresenters.Clear();

            var limit = provider.Asset.MaxConsumables;
            var inventoryPreviewPrefab = provider.Asset.InventoryPreviewPrefab;

            int cellCount = 0;

            foreach (var consumable in player.consumableStorage.GetAllItems())
            {
                var consData = provider.GetConsumable(consumable.Key);
                
                for (int i = 0; i < consumable.Value.CurrentValue; i++)
                {
                    bool isReserved = default;
                    
                    if (limit > 0 && consumable.Value.CurrentValue > 0)
                    {
                        limit--;
                        CreateBattlePreview(consData);
                        isReserved = true;
                    }
                    
                    InventoryPreview view = Instantiate(inventoryPreviewPrefab, contentInventory);
                    var presenter = new InventoryConsumable_ItemPreviewPresenter(isReserved, itemDetailWindow, view, consData);
                    _objectResolver.Inject(presenter);
                    inventoryPresenters.Add(presenter);
                    
                    cellCount++;
                }
            }

            if (cellCount < 8)
            {
                CreateEmptySells(8 - cellCount);
            }

            // заполняем пустые ячейки. Пока костыль т.к. нет нормальынх способов обработать выход из popupInventory + нужно сделать одно для всех
            if (inBattlePresenters.Count == 0)
            {
                inBattlePresenters.Clear();
                
                for (int i = 0; i < limit; i++)
                {
                    CreateBattlePreview(null);
                }   
            }
        }

        private void CreateEmptySells(int count)
        {
            if (count <= 0) return;
            
            var inventoryPreviewPrefab = provider.Asset.InventoryPreviewPrefab;
            
            for (int i = 0; i < count; i++)
            {
                InventoryPreview view = Instantiate(inventoryPreviewPrefab, contentInventory);
                    
                view.SetDefaultVariant();
                view.Icon.enabled = false;
                emptyItems.Add(view);
            }
        }

        private void CreateBattlePreview(ConsumableData data)
        {
            InBattlePreview view = Instantiate(provider.Asset.InBattlePreviewPrefab, contentInBattle);
            InventoryConsumable_InBattlePanelPresenter presenter = new(view, data);
            inBattlePresenters.Add(presenter);
        }

        private void HandlerConsumSelected(InventoryConsumable_ItemPreviewPresenter obj)
        {
            if (currentItem == obj) 
                return;
            
            currentItem?.OffPreview();
            currentItem = obj;
        }
        
        private void HandlerSelectItem(ConsumableData data)
        {
            if (!player.inBattleConsumablesService.IsFull)
            {
                CreateBattlePreview(data);
                currentItem.UpdatePreview();
                itemDetailWindow.SetInteractableButton(false);
                return;
            }

            currentSelectedItemData = data;
            _battlePanel.EnablePanel();
            itemDetailWindow.SetInteractableButton(false);
            
            foreach (var presenter in inBattlePresenters)
                presenter.SetPlayWaitEffect(true);

            CoroutineManager.StartCoroutine(WaitReplacementInBattleItem());
        }
        
        private IEnumerator WaitReplacementInBattleItem()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            var camera = GlobalCamera.Camera;
            var mousePosition = Input.mousePosition;
            bool isReplased = default;

            for (int i = 0; i < contentInBattle.transform.childCount; i++)
            {
                var child = contentInBattle.transform.GetChild(i).gameObject;
                var childRect = child.transform as RectTransform;
                var worldMousePosition = camera.ScreenToWorldPoint(mousePosition);
                var localMousePosition = childRect.InverseTransformPoint(worldMousePosition);

                if (childRect.rect.Contains(localMousePosition))
                {
                    var battlePresenter = inBattlePresenters[i];
                    var oldData = battlePresenter.data;
                    battlePresenter.SetData(currentSelectedItemData);
                    player.inBattleConsumablesService.Replace(i, currentSelectedItemData);
                    currentItem.SetReserved(true);
                    
                    var unreservedItem = inventoryPresenters.Find(x => x.data == oldData);
                    unreservedItem.SetReserved(false);
                    unreservedItem.UpdatePreview();
                    
                    isReplased = true;
                    break;
                }
            }
            
            itemDetailWindow.SetInteractableButton(!isReplased);
            
            foreach (var presenter in inBattlePresenters)
                presenter.SetPlayWaitEffect(false);
            
            _battlePanel.DisablePanel();
        }
    }
}