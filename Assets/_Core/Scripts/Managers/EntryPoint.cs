using _Core.Scripts.Core.Greyness;
using PlayerScripts;
using Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public class EntryPoint : IStartable
    {
        [Inject] private Player _player;
        [Inject] private Localization localization;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private LoadingPopup _loadingPopup;
        [Inject] private SoundManager _soundManager;
        [Inject] private GreynessManager _greynessManager;

        public void Start()
        {
            Time.timeScale = 1;
            
            Screen.SetResolution(1920, 1080, true);
            
            _greynessManager.Initialize();
            
            _soundManager.Initialize();
            
            localization.SetLanguage("Русский");
            localization.UpdateLocalization();
            
            _player.ResetPlayerData();

            _loadingPopup.Initialize();

            _sceneLoader.Initialize();
            _sceneLoader.Load(SceneEnum.Menu);
            
            _soundManager.PlayTheme(_soundManager.SoundList.mainTheme);
        }
    }
}