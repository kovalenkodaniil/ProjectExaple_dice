using System;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using Core.Localization;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class DiceInBattlePreview : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _waitEffect;
        [SerializeField] private DiceUIPreview _dicePreview;
        [SerializeField] private TooltipWindow _tooltip;
        
        public event Action OnInBattlePreviewClicked;

        private bool _isInteractable;
        private bool _isTooltipEnabled;
        private Tween _tween;
        
        public void SetDice(DiceConfig config, bool isFirstUpdate = true)
        {
            _dicePreview.InitializeInventory(config, 67);
        }
        
        public void PlayHideAnimation(Action callback = null)
        {
            _dicePreview.PlayHideAnimation(callback);
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