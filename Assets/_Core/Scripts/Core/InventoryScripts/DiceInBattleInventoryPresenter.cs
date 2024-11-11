using System;
using Core.Data;
using PlayerScripts;

namespace Core.InventoryScripts
{
    public class DiceInBattleInventoryPresenter
    {
        private DiceInBattlePreview _dicePreview;
        private DiceData _diceData;
        private DiceData _newDiceData;
        private DiceConfig _diceConfig;
        private Player _player;

        public event Action OnDiceAdded;

        public DiceInBattleInventoryPresenter(DiceInBattlePreview dicePreview, DiceData data, Player player)
        {
            _diceData = data;
            _diceConfig = _diceData.config;
            _player = player;
            _dicePreview = dicePreview;
        }

        public void Initialize()
        {
            UpdatePreview();

            _diceData.OnUpgraded += UpgradeDice;
        }

        public void PlayHideAnimation()
        {
            _dicePreview.PlayHideAnimation();
            
            _diceData.OnUpgraded -= UpgradeDice;
        }

        public void Enable(DiceData newDice)
        {
            _newDiceData = newDice;
            
            UpdatePreview();
            _dicePreview.SetWaitEffect(true);

            _dicePreview.OnInBattlePreviewClicked += UpdateDiceBattle;
        }

        private void UpgradeDice()
        {
            _diceConfig = _diceData.config;
            
            _dicePreview.PlayHideAnimation(() =>
            {
                UpdatePreview();
            });
        }

        public void DisableSelecting()
        {
            _dicePreview.SetWaitEffect(false);
            
            _dicePreview.OnInBattlePreviewClicked -= UpdateDiceBattle;
        }
        
        public void Disable()
        {
            DisableSelecting();
            _diceData.OnUpgraded -= UpgradeDice;
        }

        private void UpdateDiceBattle()
        {
            _player.RemoveDiceFromBattle(_diceData);
            _player.AddDiceInBattle(_newDiceData);

            _diceConfig = _newDiceData.config;
            _diceData = _newDiceData;

            _dicePreview.PlayHideAnimation(() =>
            {
                UpdatePreview();
            
                OnDiceAdded?.Invoke();
            });
        }

        private void UpdatePreview()
        {
            _dicePreview.SetDice(_diceConfig);
        }
    }
}