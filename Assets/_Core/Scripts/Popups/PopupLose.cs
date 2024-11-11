using System;
using Core.Localization;
using DG.Tweening;
using Managers;
using Popups;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Core.Scripts.Popups
{
    public class PopupLose : MonoBehaviour
    {
        [Inject] private PopupEffector _popupEffector;
        [Inject] private Managers.Localization localization;
        [Inject] private FontSettings _fontSettings;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private SoundManager _soundManager;
        
        public event Action OnBattleRetry;
        
        [SerializeField] private GameObject _container;
        
        [Header("AnimationComponent")] 
        [SerializeField] private CanvasGroup _header;
        [SerializeField] private Image _background;
        [SerializeField] private CanvasGroup _textPanel;

        public void Open()
        {
            PlayAppearanceAnimation();
            _container.SetActive(true);
        }
        
        public void Close()
        {
            _popupEffector.PlayTransitionEffect(() =>
            {
                OnBattleRetry?.Invoke();

                _container.SetActive(false);
            });
        }

        public void BackToMenu()
        {
            _soundManager.PlayTheme(_soundManager.SoundList.mainTheme);
            _sceneLoader.Load(SceneEnum.Menu);
        }

        public void RetryBattle()
        {
            Close();
        }

        private void PlayAppearanceAnimation()
        {
            Sequence animationOrder = DOTween.Sequence();
            
            _header.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            _textPanel.alpha = 0;
            _header.alpha = 0;
            
            
            animationOrder.Append(_header.transform.DOScale(1, 0.45f));
            animationOrder.Join(_header.DOFade(1, 0.3f));
            animationOrder.Join(_background.DOFade(0.7f, 0.25f));
            
            animationOrder.Append(_textPanel.DOFade(1, 0.4f));
        }
    }
}