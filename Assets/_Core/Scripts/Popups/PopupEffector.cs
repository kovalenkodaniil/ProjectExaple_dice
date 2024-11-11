using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Popups
{
    public class PopupEffector : MonoBehaviour
    {
        private const float SCREEN_OPEN_DURATION = 0.25f;
        private const float SCREEN_CLOSE_DURATION = 0.2f;

        [SerializeField] private CanvasGroup _transitionEffect;

        private Tween _transitionAnimation;

        public void PlayPopupOpenAnimation(CanvasGroup screen, Image background, Action callback = null)
        {
            screen.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            screen.alpha = 0;

            Sequence animationOrder = DOTween.Sequence();

            float backgroundFade = background.color.a;
            background.color = new Color(0, 0, 0, 0);
            animationOrder.Append(background.DOFade(backgroundFade, SCREEN_OPEN_DURATION));
            
            animationOrder.Join(screen.transform.DOScale(1.08f, SCREEN_OPEN_DURATION));
            animationOrder.Join(screen.DOFade(1, SCREEN_OPEN_DURATION));
            
            animationOrder.Append(screen.transform.DOScale(1f, SCREEN_OPEN_DURATION));
            
            animationOrder.OnComplete(() => callback?.Invoke());
        }

        public void PlayPopupCloseAnimation(CanvasGroup screen, Image background, Action callback = null)
        {
            Sequence animationOrder = DOTween.Sequence();
            
            float backgroundFade = background.color.a;
            animationOrder.Append(background.DOFade(0f, SCREEN_CLOSE_DURATION));
            
            animationOrder.Join(screen.transform.DOScale(0.85f, SCREEN_CLOSE_DURATION));
            animationOrder.Join(screen.DOFade(0, SCREEN_CLOSE_DURATION));
            
            animationOrder.OnComplete(() =>
            {
                callback?.Invoke();
                background.color = new Color(background.color.r, background.color.g, background.color.b,
                    backgroundFade);
            });
        }

        public void PlayHeaderAnimation(CanvasGroup header, Image background, Action callback = null)
        {
            header.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            header.alpha = 0;
            
            background.color = new Color(0, 0, 0, 0);
            
            Sequence animationOrder = DOTween.Sequence();
            
            animationOrder.Append(background.DOFade(0.7f, SCREEN_OPEN_DURATION));
            
            animationOrder.Join(header.transform.DOScale(1.15f, SCREEN_OPEN_DURATION));
            animationOrder.Join(header.DOFade(1, SCREEN_OPEN_DURATION));
            
            animationOrder.Append(header.transform.DOScale(1f, SCREEN_OPEN_DURATION));
            
            animationOrder.OnComplete(() => callback?.Invoke());
        }

        public void PlayWindowAnimation(CanvasGroup window, Action callback = null)
        {
            window.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            window.alpha = 0;
            
            Sequence animationOrder = DOTween.Sequence();
            
            animationOrder.Append(window.transform.DOScale(1.15f, SCREEN_OPEN_DURATION));
            animationOrder.Join(window.DOFade(1, SCREEN_OPEN_DURATION));
            
            animationOrder.Append(window.transform.DOScale(1f, SCREEN_OPEN_DURATION));
            
            animationOrder.OnComplete(() => callback?.Invoke());
        }
        
        public void PlayRewardCloseAnimation(CanvasGroup header, Action callback = null)
        {
            Sequence animationOrder = DOTween.Sequence();

            animationOrder.Join(header.transform.DOScale(0.85f, SCREEN_CLOSE_DURATION));
            animationOrder.Join(header.DOFade(0, SCREEN_CLOSE_DURATION));

            animationOrder.OnComplete(() => callback?.Invoke());
        }

        public void PlayWindowCloseAnimation(CanvasGroup window, Action callback = null)
        {
            Sequence animationOrder = DOTween.Sequence();

            animationOrder.Append(window.transform.DOScale(0.85f, SCREEN_CLOSE_DURATION));
            animationOrder.Join(window.DOFade(0, SCREEN_CLOSE_DURATION));

            animationOrder.OnComplete(() => callback?.Invoke());
        }

        public void PlayBackgroundHideAnimation(Image background, Action callback = null)
        {
            background.DOFade(0f, SCREEN_OPEN_DURATION).OnComplete(() => callback?.Invoke());
        }

        public void PlayTransitionEffect(Action onTransitionEnd = null, float duration = 0.15f)
        {
            _transitionAnimation?.Kill();
            
            _transitionEffect.gameObject.SetActive(true);
            _transitionEffect.alpha = 0f;
            
            _transitionAnimation = _transitionEffect.DOFade(0.7f, duration).OnComplete(() =>
            {
                onTransitionEnd?.Invoke();
                PlayViewOpenAnimation(duration);
            });
        }

        public void PlayViewOpenAnimation(float duration = 0.15f)
        {
            _transitionAnimation?.Kill();
            
            _transitionEffect.gameObject.SetActive(true);
            _transitionEffect.alpha = 0.7f;

            _transitionAnimation = _transitionEffect.DOFade(0f, duration).OnComplete(() => _transitionEffect.gameObject.SetActive(false));
        }

        public void PlayTransitionSceneEffect(Action onTransitionEnd = null, float duration = 0.15f)
        {
            _transitionAnimation?.Kill();

            _transitionEffect.gameObject.SetActive(true);
            _transitionEffect.alpha = 0f;
            
            _transitionAnimation = _transitionEffect.DOFade(0.9f, duration).OnComplete(() =>
            {
                onTransitionEnd?.Invoke();
                _transitionAnimation = _transitionEffect
                    .DOFade(0f, duration)
                    .OnComplete(() => _transitionEffect.gameObject.SetActive(false));
            });
        }
    }
}