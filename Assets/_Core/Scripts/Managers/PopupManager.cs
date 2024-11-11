using Core.InventoryScripts;
using Popups;
using UnityEngine;
using VContainer;

namespace Managers
{
    public enum EnumPopup
    {
        Character = 0,
        Settings = 1,
        Pause = 2
    }

    public class PopupManager : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PopupEffector _popupEffector;
        
        [SerializeField] public InventoryPopup _inventoryPopup;
        //[SerializeField] private PausePopup _pausePopup;

        public void OpenPopup(EnumPopup popup)
        {
            switch (popup)
            {
                case EnumPopup.Character:
                    _popupEffector.PlayTransitionEffect(() => _inventoryPopup.Open());
                    break;
                

                case EnumPopup.Pause:
                    
                    /*if (_sceneLoader.ActiveScene is SceneEnum.Game or SceneEnum.Dialog)
                        _pausePopup.OpenToMapVariant();
                    else
                        _pausePopup.OpenGalleryVariant();*/

                    break;
            }
        }
    }
}