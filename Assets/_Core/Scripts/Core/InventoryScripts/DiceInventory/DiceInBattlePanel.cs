using System;
using System.Collections.Generic;
using Core.Data;
using PlayerScripts;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class DiceInBattlePanel : MonoBehaviour
    {
        private List<DiceInBattleInventoryPresenter> _diceInBattlePresenters;
        private Player _player;

        [SerializeField] private Transform _diceInBattleParent;
        [SerializeField] private DiceInBattlePreview _diceInBattlePreviewPrefab;
        [SerializeField] private InventoryBattlePanel _battlePanelView;

        private InventorClickHolder _clickHolder;
        
        public void Initialize(Player player, InventorClickHolder clickHolder)
        {
            _player = player;
            _clickHolder = clickHolder;

            CreateDiceInBattle();
        }

        public void OnDisable()
        {
            _diceInBattlePresenters.ForEach(dice =>
            {
                dice.OnDiceAdded -= DisableBattlePanel;
                dice.Disable();
            });
        }

        public void Hide()
        {
            DisableBattlePanel();
            
            _diceInBattlePresenters.ForEach(presenter =>
            {
                presenter.PlayHideAnimation();
            });
        }

        public void Show()
        {
            _diceInBattlePresenters.ForEach(presenter =>
            {
                presenter.Initialize();
                presenter.OnDiceAdded += DisableBattlePanel;
            });
        }

        public void EnableSelectingMode(DiceData diceData)
        {
            _diceInBattlePresenters.ForEach(dice => dice.Enable(diceData));
        }

        public void DisableSelectingMode()
        {
            _diceInBattlePresenters.ForEach(dice => dice.DisableSelecting());
        }

        public void DisableBattlePanel()
        {
            _battlePanelView.DisablePanel();
            
            DisableSelectingMode();
        }

        private void CreateDiceInBattle()
        {
            _diceInBattlePresenters = new List<DiceInBattleInventoryPresenter>();
            
            _player.diceInBattle.ForEach(dice =>
            {
                DiceInBattlePreview dicePreview = Instantiate(_diceInBattlePreviewPrefab, _diceInBattleParent);

                DiceInBattleInventoryPresenter diceInventoryPresenter =
                    new DiceInBattleInventoryPresenter(dicePreview, dice, _player);

                _diceInBattlePresenters.Add(diceInventoryPresenter);
                
                _clickHolder.AddDicePreview(dicePreview);
            });
        }
    }
}