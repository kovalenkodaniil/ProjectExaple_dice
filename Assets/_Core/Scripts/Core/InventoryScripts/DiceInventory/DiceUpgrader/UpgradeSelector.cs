using System;
using System.Collections.Generic;
using Core.Data;
using Managers;
using PlayerScripts;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Core.InventoryScripts
{
    public class UpgradeSelector : MonoBehaviour
    {
        //[Inject] private PopupEffector _popupEffector;
        [Inject] private Player _player;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _background;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _diceName;
        [SerializeField] private List<DiceUpgradeView> _upgradeViews;
        [SerializeField] private BuyButtonView _updgradeButton;

        private DiceData _diceForUpgrade;
        private DiceConfig _selectedUpgrade;
        private DiceUpgradeView _currentView;

        public event Action OnClose;
        public event Action<DiceConfig> OnUpgradeApproved;

        public void Open(DiceData diceData)
        {
            //_popupEffector.PlayPopupOpenAnimation(_canvasGroup, _background);
            _container.SetActive(true);

            _diceForUpgrade = diceData;
            _updgradeButton.SetInteractable(false);
            _updgradeButton.SetCost(_diceForUpgrade.config.upgradeCost);
            
            SetDiceName();
            SetUpgradeVariant();
        }
        
        public void Close()
        {
            /*_popupEffector.PlayPopupCloseAnimation(_canvasGroup, _background, () =>
            {
                _container.SetActive(false);
            
                OnClose?.Invoke();
            });*/
            
            _container.SetActive(false);
            
            OnClose?.Invoke();
            
            _upgradeViews.ForEach(view => view.OnSelected -= OnVariantSelected);
        }

        public void Upgrade()
        {
            Close();

            _player.currencyStorage.TrySpend(EnumCurrency.MONEY, _diceForUpgrade.config.upgradeCost);
            
            _diceForUpgrade.Upgrade(_selectedUpgrade);
            
            OnUpgradeApproved?.Invoke(_selectedUpgrade);
        }
        
        private void SetDiceName() => _diceName.text = _diceForUpgrade.config.diceName;

        private void SetUpgradeVariant()
        {
            _upgradeViews.ForEach(view => view.gameObject.SetActive(false));
            
            for (int i = 0; i < _diceForUpgrade.config.Upgrades.Count; i++)
            {
                _upgradeViews[i].gameObject.SetActive(true);
                
                _upgradeViews[i].SetDice(_diceForUpgrade.config.Upgrades[i]);
                _upgradeViews[i].SetDiceName(_diceForUpgrade.config.Upgrades[i].diceName);
                _upgradeViews[i].CreateTooltip();
                _upgradeViews[i].Deselect();

                _upgradeViews[i].OnSelected += OnVariantSelected;
            }
        }

        private void OnVariantSelected(DiceUpgradeView view, DiceConfig diceConfig)
        {
            if (_currentView != view)
                _currentView?.Deselect();

            _currentView = view;
            _selectedUpgrade = diceConfig;
            _updgradeButton.SetInteractable(true);
        }
    }
}