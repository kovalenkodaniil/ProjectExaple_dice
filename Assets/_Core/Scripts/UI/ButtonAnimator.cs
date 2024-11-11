using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UI
{
    public class ButtonAnimator : MonoBehaviour
    {
        private TweenerCore<Vector3, Vector3, VectorOptions> Animation { get; set; }
        
        private void OnDestroy()
        {
            DestroyAnimation();
        }
        
        public void PlayPressAnimation(RectTransform rectTransform, Action callback = null)
        {
            if (Animation != null && Animation.active) return;
            
            Animation = rectTransform.DOScale(-0.1f, 0.1f).SetRelative().SetLoops(2, LoopType.Yoyo);
            Animation.OnComplete(()=>
            {
                callback?.Invoke();
                DestroyAnimation();
            });
        }

        private void DestroyAnimation()
        {
            Animation?.Kill();
        }
    }
}