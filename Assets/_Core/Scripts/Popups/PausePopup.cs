using System;
using Core.Localization;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Popups
{
    public class PausePopup : MonoBehaviour
    {
        [Inject] private PopupManager _popupManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PopupEffector _popupEffector;
        [Inject] private Managers.Localization localization;
        [Inject] private FontSettings _fontSettings;

        public static event Action OnClickMainMenu;

        [SerializeField] private CanvasGroup _screen;
        [SerializeField] private Image _bacground;
        [SerializeField] private GameObject _container;

        public void OpenGalleryVariant()
        {
            _container.SetActive(true);

            _popupEffector.PlayPopupOpenAnimation(_screen, _bacground);
        }

        public void OpenToMapVariant(bool isBackToMapAvailable = true)
        {
            _container.SetActive(true);

            _popupEffector.PlayPopupOpenAnimation(_screen, _bacground);
        }

        public void Close()
        {
            _popupEffector.PlayPopupCloseAnimation(_screen, _bacground, () => _container.SetActive(false));
        }

        public void OpenSetting()
        {
            _popupManager.OpenPopup(EnumPopup.Settings);
            Close();
        }

        public void BackToMainMenu()
        {
            OnClickMainMenu?.Invoke();
            Close();
        }

        public void BackToMap()
        {
            _sceneLoader.Load(SceneEnum.Map);
            Close();
        }
    }
}