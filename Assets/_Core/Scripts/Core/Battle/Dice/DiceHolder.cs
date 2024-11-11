using System;
using Core.Data;
using DG.Tweening;
using UnityEngine;

namespace _Core.Scripts.Core.Battle.Dice
{
    public class DiceHolder : MonoBehaviour
    {
        public event Func<bool> tryToHold;
        public event Func<bool> tryToUnhold;
        
        public event Action OnHoldFinished;
        public event Action OnUnHoldFinished;

        public Tween flyAnimation;
        
        private Transform _diceTransform;
        private DiceSettings _diceSettings;
        private float _holdPositionZ;
        private bool _isAnimationPlaying;
        
        public bool IsHeld { get; private set; }
        public bool CanBeHold { get; set; }
        public bool IsBlocking { get; set; }

        public void Initialize(DiceSettings diceSettings, Transform diceTransform)
        {
            _diceSettings = diceSettings;
            _holdPositionZ = diceTransform.localPosition.z;
            _diceTransform = diceTransform;

            CanBeHold = true;
        }

        public void ResetFlyAnimation()
        {
            flyAnimation?.Kill();
        }

        public void UnHold(Action callback = null)
        {
            if (IsBlocking)
            {
                return;
            }
            
            IsHeld = false;
            _isAnimationPlaying = true;
            
            _diceTransform.DOLocalMoveZ(_holdPositionZ, _diceSettings.diceHoldTime)
                .OnComplete(() =>
                {
                    _isAnimationPlaying = false;
                    OnUnHoldFinished?.Invoke();
                    
                    if (callback == null)
                        PlayFlyAnimation();
                    else
                        callback.Invoke();
                });
        }

        public void BlockDice(bool isBlocking)
        {
            IsBlocking = isBlocking;
        }

        private void PlayFlyAnimation()
        {
            flyAnimation = _diceTransform.DOLocalMoveZ(_diceSettings.diceFloatDelta, _diceSettings.diceFloatSpeed)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }
    }
}