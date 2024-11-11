using System;
using Core.Data;
using PlayerScripts;

namespace Core.InventoryScripts
{
    public class DiceInventoryPresenter
    {
        public event Action<DiceInventoryPresenter, DiceData> OnDiceSelected;
        
        private DiceInventoryPreview _dicePreview;
        private Player _player;
        private DiceData _diceData;
        private DiceConfig _diceConfig;

        private bool _isSelected;

        public DiceInventoryPresenter(DiceData diceData, DiceInventoryPreview dicePreview, Player player)
        {
            _diceData = diceData;
            _diceConfig = diceData.config;
            _dicePreview = dicePreview;
            _player = player;
        }

        public void Enable()
        {
            ShowDice();
            
            _isSelected = false;

            _diceData.OnUpgraded += UpgradeDice;
            _diceData.OnAddedInBattle += ShowDice;
            _diceData.OnRemovedFromBattle += ShowDice;
            _dicePreview.OnPreviewClicked += SelectPreview;
        }

        public void Disable()
        {
            _dicePreview.PlayHideAnimation();
            
            _diceData.OnUpgraded -= UpgradeDice;
            _diceData.OnAddedInBattle -= ShowDice;
            _diceData.OnRemovedFromBattle -= ShowDice;
            _dicePreview.OnPreviewClicked -= SelectPreview;
        }

        public void SelectPreview()
        {
            if (_isSelected) return;
            
            _isSelected = true;

            _dicePreview.SetSelectingVariant();

            OnDiceSelected?.Invoke(this, _diceData);
        }

        public void OffPreview()
        {
            _isSelected = false;
            
            ShowDice();
            _dicePreview.HideSelectingEffect();
            _dicePreview.SetDiceLevel(_diceConfig.levelNumber);
        }

        private void ShowDice()
        {
            if (_player.diceInBattle.Contains(_diceData))
                _dicePreview.SetInBattleVariant();
            else
                _dicePreview.SetDefaultVariant();

            _dicePreview.SetDiceLevel(_diceConfig.levelNumber);
            
            _dicePreview.SetDice(_diceConfig);

            if (_diceData.quantity > 1) _dicePreview.SetDiceCount(_diceData.quantity);
        }

        private void UpgradeDice()
        {
            _diceConfig = _diceData.config;

            _dicePreview.PlayUpgradeAnimation(() =>
            {
                ShowDice();
                _dicePreview.PlayNewDiceAnimation();
            });
        }
    }
}