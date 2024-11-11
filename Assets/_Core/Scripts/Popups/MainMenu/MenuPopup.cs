using _Core.Scripts.Core.Data;
using _Core.Scripts.Core.Greyness;
using Managers;
using PlayerScripts;
using TMPro;
using UnityEngine;
using VContainer;

namespace Popups
{
    public class MenuPopup : MonoBehaviour
    {
        [Inject] private Player _player;
        [Inject] private GameDataConfig _gameDataConfig;
        [Inject] private PopupManager _popupManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PopupEffector _popupEffector;
        [Inject] private IObjectResolver objectResolver;
        [Inject] private GreynessPresenter _greynessPresenter;

        private MenuPresenter presenter = new();
        
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text lbPlay;
        [SerializeField] private TMP_Text lbTalents;
        
        public void Start()
        {
            objectResolver.Inject(presenter);
            
            _container.SetActive(true);
            
            _greynessPresenter.Initialize();
        }

        public void Open()
        {
            _container.SetActive(true);

            lbPlay.text = Managers.Localization.Translate(LocalizationKeys.menu_play);
            lbTalents.text = Managers.Localization.Translate(LocalizationKeys.menu_talents);
        }

        public void Close()
        {
            _container.SetActive(false);
        }

        public void OpenInventory() => _popupManager.OpenPopup(EnumPopup.Character);
        
        public void ClickBattle() => presenter.OnClickBattle();

        public void ClickUpgrade() => presenter.OnClickUpgrade();
        
        public void ExitGame() => Application.Quit();
    }
}