using System;
using UnityEngine;

namespace _Core.Scripts.Core.Battle
{
    public enum TurnState
    {
        Start = 0,
        RollDice = 1,
        RollResult = 2,
        PlayerAttack = 3,
        EnemyAttack = 4,
        End = 5
    }
    
    public class TurnManager : MonoBehaviour
    {
        public event Action OnStartTurn;
        public event Action OnRollDice;
        public event Action OnRollResult;
        public event Action OnPlayerAttack;
        public event Action OnEnemyAttack;
        public event Action OnEndTurn;
        
        private int _stateAmount = 6;
        
        public TurnState currentState { get; private set; }

        public void Initialize()
        {
            _stateAmount = Enum.GetNames(typeof(TurnState)).Length;
            currentState = TurnState.Start;
        }
        
        public void SkipTurn()
        {
            currentState = (TurnState)(((int)_stateAmount - 1) % _stateAmount);
            print(currentState);
            InvokeCurrentState();
        }

        public void NextState()
        {
            currentState = (TurnState)(((int)currentState + 1) % _stateAmount);
            //print(currentState);
            InvokeCurrentState();
        }

        public void InvokeCurrentState()
        {
            switch (currentState)
            {
                case TurnState.Start:
                    OnStartTurn?.Invoke();
                    break;
                case TurnState.RollDice:
                    OnRollDice?.Invoke();
                    break;
                case TurnState.RollResult:
                    OnRollResult?.Invoke();
                    break;
                case TurnState.PlayerAttack:
                    OnPlayerAttack?.Invoke();
                    break;
                case TurnState.EnemyAttack:
                    OnEnemyAttack?.Invoke();
                    break;
                case TurnState.End:
                    OnEndTurn?.Invoke();
                    break;
            }
        }
    }
}