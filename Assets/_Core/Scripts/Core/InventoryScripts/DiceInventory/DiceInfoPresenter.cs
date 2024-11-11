using System;
using Core.Data;
using PlayerScripts;

namespace Core.InventoryScripts
{
    public class DiceInfoPresenter
    {
        public event Action<DiceData> OnDiceSelectedForBattle;
        public event Action<DiceData> OnDiceTryUpgraded;
        
        private DiceInventoryView _inventoryView;
        private Player _player;
        private DiceData _diceData;
        private DiceConfig _diceConfig;
        
        public DiceInfoPresenter(DiceInventoryView view, Player player)
        {
            _player = player;
            _inventoryView = view;
        }

        public void Enable()
        {
            _inventoryView.OnDiceSelected += SelectDice;
            _inventoryView.OnUpgradeClicked += TryUpgradeDice;
        }

        public void Disable()
        {
            _inventoryView.OnDiceSelected -= SelectDice;
            _inventoryView.OnUpgradeClicked -= TryUpgradeDice;
        }

        public void ShowDiceInfo(DiceData diceData)
        {
            if (_diceData != null)
            {
                _diceData.OnUpgraded -= PlayUpgradeAnimation;
                _diceData.OnAddedInBattle -= UpdateSelectButton;
                
                _diceData = diceData;
                _diceConfig = _diceData.config;
                _inventoryView.PlayHideContent(UpdateView);
            }
            else
            {
                _diceData = diceData;
                _diceConfig = _diceData.config;
                UpdateView();
            }
            
            _diceData.OnUpgraded += PlayUpgradeAnimation;
            _diceData.OnAddedInBattle += UpdateSelectButton;
        }
        
        public void UpdateSelectButton()
        {
            _inventoryView.SetEnableSelectButton(!_player.diceInBattle.Contains(_diceData));
        }

        private void PlayUpgradeAnimation()
        {
            _inventoryView.PlayFadeAnimation(() => _inventoryView.SetDicePreview(_diceData.config));
            
            int updgradeEdgeIndex = 0;

            for (int i = 0; i < _diceData.config.Edges.Count; i++)
            {
                if (_diceConfig.Edges[i] != _diceData.config.Edges[i])
                {
                    _inventoryView.PlayUpgradeAnimation(i, _diceData.config.Edges[updgradeEdgeIndex]);
                }
            }

            _inventoryView.PlayChangeLevelAnimation(_diceData.config.levelNumber);
            
            UpdateUpgradeButton();
        }

        private void UpdateView()
        {
            _inventoryView.SetName(_diceData.config.diceName);
            _inventoryView.SetLevel(_diceData.config.levelNumber);
            _inventoryView.SetDescription(_diceData.config.description);
            _inventoryView.SetCurrentEdges(_diceData.config.Edges);
            _inventoryView.SetDicePreview(_diceData.config);
            
            _inventoryView.PlayAppearanceContent();
            
            UpdateSelectButton();
            UpdateUpgradeButton();
        }

        private void UpdateUpgradeButton()
        {
            if (_diceData.config.Upgrades.Count > 0)
            {
                _inventoryView.SetUpgradeCost(_diceData.config.upgradeCost);
                
                if (_player.currencyStorage.Count(EnumCurrency.MONEY) < _diceData.config.upgradeCost)
                    _inventoryView.DisableUpgradeButton();
            }
            else
                _inventoryView.SetMaxLevelUpgradeButton();
        }

        private void SelectDice() => OnDiceSelectedForBattle?.Invoke(_diceData);

        private void TryUpgradeDice() => OnDiceTryUpgraded?.Invoke(_diceData);
    }
}