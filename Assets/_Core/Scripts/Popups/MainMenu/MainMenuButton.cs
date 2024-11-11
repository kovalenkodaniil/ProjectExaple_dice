using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Popups.MainMenu
{
    public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _backround;
        [SerializeField] private Sprite _defaultBackground;
        [SerializeField] private Sprite _selectedBackground;
        [SerializeField] private TMP_Text _defauultText;
        [SerializeField] private TMP_Text _activeText;

        private RectTransform _rect;
        private Tween tween;
        private Vector2 _startPosition;
        private float _offset = 13f;
        
        public void Awake()
        {
            _rect = gameObject.GetComponent<RectTransform>();
            _startPosition = _rect.anchoredPosition;
        }

        private void OnEnable()
        {
            _rect.anchoredPosition = _startPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //_backround.sprite = _selectedBackground;
            
            //_activeText.gameObject.SetActive(true);
            //_defauultText.gameObject.SetActive(false);
            
            if(tween is {active: true})
                tween.Kill();

            tween = _rect.DOAnchorPosX(_offset, 0.1f).SetRelative();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //_backround.sprite = _defaultBackground;
            
            //_activeText.gameObject.SetActive(false);
            //_defauultText.gameObject.SetActive(true);
            
            if(tween is {active: true})
                tween.Kill();

            tween = _rect.DOAnchorPosX(_startPosition.x, 0.1f);
        }
    }
}