using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Data;
using Core.InventoryScripts.Items;
using Core.PreBattle;
using PlayerScripts;
using Popups;
using UnityEngine;
using VContainer;

namespace Core.InventoryScripts
{
    public class InventoryPopup : MonoBehaviour, ITabGroup
    {
        [Inject] private Player _player;
        [Inject] private GameDataConfig _gameStaticData;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private Managers.Localization localization;
        [Inject] private PopupEffector _popupEffector;

        public static event Action OnStaticClose;
        public event Action OnClosed;

        private InventoryMoneyObserver moneyObserver;

        [SerializeField] private GameObject _container;
        [SerializeField] private TextView moneyView;
        [SerializeField] private List<InventoryTabButton> tabButtons;

        public List<ITab> Tabs { get; set; }
        private ITab _currentTab;
        
        public enum EIntentoryTab { Dices, Spells, Consumables, Items }

        public void Open()
        {
            _container.SetActive(true);
            
            if (Tabs == null)
            {
                Tabs = new List<ITab>
                {
                    GetComponentInChildren<InventorySkills_InventoryPanel>(),
                    GetComponentInChildren<DiceInventoryPanel>(),
                    GetComponentInChildren<InventoryConsumable_InventoryPanel>(),
                    GetComponentInChildren<InventoryItems_InventoryPanel>()
                };
                
                Tabs.ForEach(tab =>
                {
                    tab.Initialize(_player);
                    _objectResolver.Inject(tab);
                });

                SelectTabs(Tabs[0]);
            }
            else
            {
                _currentTab?.Open();
            }

            moneyObserver ??= new InventoryMoneyObserver(moneyView, _player.currencyStorage.GetItem(EnumCurrency.MONEY));
            moneyObserver.OnEnable();
            
            foreach (var btn in tabButtons)
            {
                btn.Text = btn.Type switch
                {
                    EIntentoryTab.Dices => Managers.Localization.Translate(LocalizationKeys.popupInventory_btnDices),
                    EIntentoryTab.Spells => Managers.Localization.Translate(LocalizationKeys.popupInventory_btnSpells),
                    EIntentoryTab.Consumables => Managers.Localization.Translate(LocalizationKeys.popupInventory_btnConsumables),
                    EIntentoryTab.Items => Managers.Localization.Translate(LocalizationKeys.popupInventory_btnItems),
                    _ => string.Empty
                };
            }

            SetActiveTabButton(EIntentoryTab.Spells);
        }

        public void Close()
        {
            moneyObserver.OnDisable();
            
            OnClosed?.Invoke();
            OnStaticClose?.Invoke();
            _currentTab?.Close();
            
            _popupEffector.PlayTransitionEffect(() => _container.SetActive(false));
        }
        
        public void SelectTabs(ITab target)
        {
            if (_currentTab != null)
            {
                _currentTab.OnClose += OpenTab;
                _currentTab?.Close();
            }
            else
            {
                target.Open();
            }

            _currentTab = target;
        }
        
        public void OpenTab(ITab target)
        {
            target.OnClose -= OpenTab;
            _currentTab.Open();
        }

        public void OnClickDices()
        {
            SetActiveTabButton(EIntentoryTab.Dices);
            SelectTabs(Tabs.Find(tab => tab is DiceInventoryPanel));
        }
        
        public void OnClickSpells()
        {
            SetActiveTabButton(EIntentoryTab.Spells);
            SelectTabs(Tabs.Find(tab => tab is InventorySkills_InventoryPanel));
        }
        
        public void OnClickConsumables()
        {
            SetActiveTabButton(EIntentoryTab.Consumables);
            SelectTabs(Tabs.Find(tab => tab is InventoryConsumable_InventoryPanel));
        }
        
        public void OnClickItems()
        {
            SetActiveTabButton(EIntentoryTab.Items);
            SelectTabs(Tabs.Find(tab => tab is InventoryItems_InventoryPanel));
        }

        private void SetActiveTabButton(EIntentoryTab tab)
        {
            foreach (var btn in tabButtons)
            {
                btn.SetEnable(btn.Type == tab);
            }
        }
    }
}