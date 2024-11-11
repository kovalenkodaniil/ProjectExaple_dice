using Core.Localization;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Popups
{
    public class LoadingPopup : MonoBehaviour
    {
        [Inject] private PopupEffector _popupEffector;
        [Inject] private FontSettings _fontSettings;

        [SerializeField] private Slider _loadBar;
        [SerializeField] private TMP_Text lbLoading;
        [SerializeField] private GameObject _container;

        private AsyncOperation _sceneLoading;
        private bool _isStopLoading;

        public void Initialize()
        {
            gameObject.SetActive(false);
        }

        public void ShowLoading(AsyncOperation sceneLoading)
        {
            _sceneLoading = sceneLoading;
            _loadBar.value = _sceneLoading.progress;
            _isStopLoading = false;

            _sceneLoading.allowSceneActivation = true;
            _sceneLoading.completed -= HideLoading;
            
            gameObject.SetActive(true);
            _container.SetActive(true);

            lbLoading.text = Managers.Localization.Translate(LocalizationKeys.popupLoading_loading);
        }

        public void Update()
        {
            if (_isStopLoading) return;
            if (_sceneLoading.isDone) HideLoading(_sceneLoading);
            
            _loadBar.value = _sceneLoading.progress;
        }

        public void Close()
        {
            _container.SetActive(false);
            gameObject.SetActive(false);
        }

        private void HideLoading(AsyncOperation sceneLoading)
        {
            _sceneLoading.completed -= HideLoading;
            
            _isStopLoading = true;
            _popupEffector.PlayTransitionEffect(()=>Close());
        }
    } 
}