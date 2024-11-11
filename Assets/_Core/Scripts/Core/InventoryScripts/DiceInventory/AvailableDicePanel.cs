using System;
using System.Collections.Generic;
using Core.Data;
using PlayerScripts;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class AvailableDicePanel : MonoBehaviour
    {
        public event Action<DiceData> OnDiceSelected;
        
        private List<DiceInventoryPresenter> _diceInventoryPresenters;
        private DiceInventoryPresenter _selectedPresenter;
        private Player _player;
        
        [SerializeField] private Transform _availableDiceParent;
        [SerializeField] private DiceInventoryPreview _dicePreviewPrefab;

        public void Initialize(Player player)
        {
            _player = player;

            CreateAvailableDice();
        }
        
        public void Open()
        {
            if (_diceInventoryPresenters.Count < _player.availableDice.Count)
                UpdateInventoryPresenters();

            _diceInventoryPresenters.ForEach(presenter =>
            {
                presenter.Enable();

                presenter.OnDiceSelected += SelectDice;
            });
            
            _diceInventoryPresenters[0].SelectPreview();
        }

        public void Close()
        {
            _diceInventoryPresenters.ForEach(presenter =>
            {
                presenter.Disable();
                
                presenter.OnDiceSelected -= SelectDice;
            });
        }

        private void CreateAvailableDice()
        {
            _diceInventoryPresenters = new List<DiceInventoryPresenter>();
            
            _player.availableDice.ForEach(dice =>
            {
                DiceInventoryPreview dicePreview = Instantiate(_dicePreviewPrefab, _availableDiceParent);

                DiceInventoryPresenter diceInventoryPresenter =
                    new DiceInventoryPresenter(dice, dicePreview, _player);

                _diceInventoryPresenters.Add(diceInventoryPresenter);
            });
        }
        
        private void SelectDice(DiceInventoryPresenter selectedPresenter, DiceData diceData)
        {
            if (_selectedPresenter == selectedPresenter) return;

            _selectedPresenter?.OffPreview();

            _selectedPresenter = selectedPresenter;
            
            OnDiceSelected?.Invoke(diceData);
        }
        
        private void UpdateInventoryPresenters()
        {
            for (int i = 0; i < _player.availableDice.Count; i++)
            {
                if (i < _diceInventoryPresenters.Count) continue;
                
                DiceInventoryPreview dicePreview = Instantiate(_dicePreviewPrefab, _availableDiceParent);
                
                DiceInventoryPresenter diceInventoryPresenter =
                    new DiceInventoryPresenter(_player.availableDice[i], dicePreview, _player);
                
                _diceInventoryPresenters.Add(diceInventoryPresenter);
            }
        }
    }
}