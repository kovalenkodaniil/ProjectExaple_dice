using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using R3;
using UnityEngine;
using Utils.Animations;

namespace Core.InventoryScripts
{
    public class InventoryMoneyObserver
    {
        private int oldValue;
        private TweenerCore<int, int, NoOptions> tween; 
        private IDisposable subscription;
        private TextView view;
        private ReadOnlyReactiveProperty<int> money;

        public InventoryMoneyObserver(TextView view, ReadOnlyReactiveProperty<int> money)
        {
            this.money = money;
            this.view = view;
        }

        public void OnEnable()
        {
            oldValue = this.money.CurrentValue;
            view.Text = oldValue.ToString();
            
            subscription = money
                .Where(value => value != oldValue)
                .Subscribe(View_UpdateText);
        }

        public void OnDisable()
        {
            subscription.Dispose();
        }

        private void View_UpdateText(int amount)
        {
            if (tween is { active: true })
            {
                tween.ChangeEndValue(amount, true);
            }
            else
            {
                Color color = oldValue > amount ? Color.red : Color.green;
                tween = CounterAnimations.InterpolateChange(view.Lb, oldValue, amount, 0.5f, color, Ease.OutQuad);    
            }
            
            oldValue = amount;
        }
    }
}