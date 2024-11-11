using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace Utils.Animations
{
    public static class CounterAnimations
    {
        public static TweenerCore<int, int, NoOptions> InterpolateChange(TMP_Text lb, int startValue, int targetValue, float duration, 
            Color color, Ease ease = Ease.Linear, string format = "")
        {
            if (string.IsNullOrEmpty(format))
                format = "{0}";

            var originColor = lb.color;
            lb.color = color;

            return DOTween.To(() => startValue, x => lb.text = string.Format(format, x, Mathf.RoundToInt(x)), targetValue, duration)
                .SetEase(ease)
                .SetLink(lb.gameObject)
                .OnKill(() => lb.color = originColor);
        }
    }
}