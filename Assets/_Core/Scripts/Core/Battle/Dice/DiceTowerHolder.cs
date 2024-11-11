using System;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.Dice
{
    public class DiceTowerHolder : MonoBehaviour
    {
        private int _holdCount;
        private List<DiceHolder> _diceHolders;
        private Player _player;
        
        public event Action<int> OnHoldCountChange;
        public event Action OnDiceUnholded;
        public event Action OnDiceHolded;

        public void Initialize(List<Dice> diceList, Player player)
        {
            _diceHolders = new List<DiceHolder>();
            _player = player;
            
            diceList.ForEach(dice => _diceHolders.Add(dice.diceHolder));

            _diceHolders.ForEach(holder =>
            {
                holder.tryToHold += HoldDice;
                holder.tryToUnhold += UnHoldDice;
            });

            _holdCount = _player.FixationComponent.MaxValue;
            OnHoldCountChange?.Invoke(_holdCount);
        }

        public void OnDisable()
        {
            _diceHolders?.ForEach(holder =>
            {
                holder.tryToHold -= HoldDice;
                holder.tryToUnhold -= UnHoldDice;
            });
        }
        
        public void PrepareDiceForNewTurn()
        {
            _diceHolders.ForEach(dice =>
            {
                dice.CanBeHold = true;
                dice.UnHold();
            });
        }
        
        public void IncreaseHoldCounter(int value = 1)
        {
            _holdCount += value;
            OnHoldCountChange?.Invoke(_holdCount);
        }

        private bool UnHoldDice()
        {
            _holdCount++;

            OnHoldCountChange?.Invoke(_holdCount);
            OnDiceUnholded?.Invoke();
            return true;
        }

        private bool HoldDice()
        {
            if (_player.isStuned) return false;
            if (_holdCount <= 0) return false;

            _holdCount--;
            
            OnHoldCountChange?.Invoke(_holdCount);
            OnDiceHolded?.Invoke();
            return true;
        }
    }
}