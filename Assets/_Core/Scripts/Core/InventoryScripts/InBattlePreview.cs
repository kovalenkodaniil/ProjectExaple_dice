using System;
using Core.Localization;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class InBattlePreview : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _waitEffect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TooltipWindow _tooltip;
        
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        
        public event Action OnInBattlePreviewClicked;

        private bool _isInteractable;
        private bool _isTooltipEnabled;
        private Tween _tween;

        private void Start()
        {
            _waitEffect.gameObject.SetActive(false);
        }

        public void SetIcon(Sprite spellIcon, bool isFirstUpdate = true)
        {
            _icon.sprite = spellIcon;
            
            _icon.gameObject.SetActive(true);

            if (!isFirstUpdate)
            {
                _canvasGroup.alpha = 0;

                Sequence animationOrder = DOTween.Sequence();

                animationOrder.Append(_canvasGroup.transform.DOScale(1.2f, 0.35f));
                animationOrder.Join(_canvasGroup.DOFade(1, 0.3f));
                animationOrder.Append(_canvasGroup.transform.DOScale(1f, 0.15f));
            }
        }

        public void SetEmptyState()
        {
            _icon.gameObject.SetActive(false);
            _waitEffect.gameObject.SetActive(false);
        }

        public void PLayHideAnimation(Action callback)
        {
            _canvasGroup.DOFade(0, 0.15f).SetDelay(0.2f);
            _canvasGroup.transform.DOScale(0, 0.35f).OnComplete(() => callback?.Invoke());
        }

        public void SetWaitEffect(bool isWaiting)
        {
            _isInteractable = isWaiting;
            
            _waitEffect.gameObject.SetActive(isWaiting);

            if (isWaiting)
                _tween = _waitEffect.DOFade(0, 0.65f).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
            else
            {
                _tween?.Kill();
                _waitEffect.color = Color.white;
            }
        }
        
        public void EnableTooltip(string tooltipText, FontSetting fontSetting)
        {
            _isTooltipEnabled = true;
            
            _tooltip.SetText(tooltipText);
            _tooltip.SetFont(fontSetting);
        }
        
        public void EnableTooltip(string tooltipText)
        {
            _isTooltipEnabled = true;
            
            _tooltip.SetText(tooltipText);
        }

        public void SelectPreview()
        {
            OnInBattlePreviewClicked?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isInteractable) return;

            SelectPreview();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isTooltipEnabled)
                _tooltip.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isTooltipEnabled)
                _tooltip.gameObject.SetActive(false);
        }
    }
}