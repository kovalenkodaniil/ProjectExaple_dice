using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonPressedAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _rect;
        
        private TweenerCore<Vector3, Vector3, VectorOptions> Animation { get; set; }

        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = _rect.localScale;
        }

        private void OnDestroy()
        {
            DestroyAnimation();
        }

        private void DestroyAnimation()
        {
            Animation?.Kill();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DestroyAnimation();
            Animation = _rect.DOScale(-0.1f, 0.1f).SetRelative().SetLink(gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            DestroyAnimation();
            Animation = _rect.DOScale(_defaultScale, 0.1f).SetLink(gameObject);
        }
    }
}