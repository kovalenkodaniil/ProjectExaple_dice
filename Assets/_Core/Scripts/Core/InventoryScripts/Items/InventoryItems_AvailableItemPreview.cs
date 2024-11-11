using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_AvailableItemPreview : MonoBehaviour, IPointerClickHandler
    {
        [Header("Icon")]
        [SerializeField] private Image _icon;
        [SerializeField] private Color _defaultIconColor;
        [SerializeField] private Color _selectingIconColor;
        
        [Header("Other")]
        [SerializeField] private Image _selectingEffect;
        [SerializeField] private Image _inBattleLabel;
        [SerializeField] private Color _lockColor;
        
        public event Action OnPreviewClicked;

        private bool _isInteractable;
        private bool _isDragable;
        
        public Image Icon => _icon;
        
        private void Start()
        {
            _selectingEffect.gameObject.SetActive(false);
        }
        
        public void SetInBattleVariant()
        {
            _isInteractable = true;
            _inBattleLabel.gameObject.SetActive(true);
        }
        
        public void SetDefaultVariant()
        {
            _isInteractable = true;
            
            _inBattleLabel.gameObject.SetActive(false);
        }

        public void HideSelectingEffect()
        {
            _selectingEffect.DOFade(0, 0.35f).SetDelay(0.05f).SetLink(gameObject);
            _icon.color = _defaultIconColor;
        }

        public void SetSelectingVariant()
        {
            _isInteractable = true;

            _icon.color = _selectingIconColor;
            
            _selectingEffect.DOFade(1, 0.35f).SetDelay(0.05f);
            
            _selectingEffect.gameObject.SetActive(true);
            _inBattleLabel.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isInteractable) return;
            
            SetSelectingVariant();
            
            OnPreviewClicked?.Invoke();
        }
    }
}