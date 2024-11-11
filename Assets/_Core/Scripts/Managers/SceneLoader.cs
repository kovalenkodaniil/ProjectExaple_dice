using System;
using System.Collections.Generic;
using Core.Saves;
using Popups;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Managers
{
    public enum SceneEnum
    {
        Initial = 0,
        Game = 1,
        Map = 2,
        Menu = 3,
        Dialog = 4,
        SexScene = 5
    }
    
    public class SceneLoader
    {
        private const float SCENE_TRANSITION_DURATION = 0.5f;
        
        [Inject] private PopupEffector _popupEffector;
        [Inject] private LoadingPopup _loadingPopup;
        
        public static SceneLoader Instance { get; private set; }
        
        private static Dictionary<SceneEnum, Action> onLoadTransitions;
        private static Dictionary<SceneEnum, Action> loadedActions;

        public SceneEnum ActiveScene;

        private AsyncOperation asyncSceneLoading;
        private List<SceneEnum> _loadedScene;
        private SceneEnum _currentLoadedScene;
        
        public event Action<SceneEnum> OnSceneLoaded;
        public event Action<SceneEnum> OnSceneUnloaded;

        public void Initialize()
        {
            onLoadTransitions = new()
            {
                { SceneEnum.Game, OnGameLoaded },
                { SceneEnum.Menu, DefaultOnLoaded },
            };
            
            loadedActions = new()
            {
                { SceneEnum.Game, DefaultLoad },
                { SceneEnum.Menu, DefaultLoad },
            };

            Instance ??= this;

            _loadedScene = new List<SceneEnum>();
            ActiveScene = SceneEnum.Initial;
        }

        public void Load(SceneEnum sceneEnum)
        {
            _currentLoadedScene = sceneEnum;
            loadedActions[sceneEnum].Invoke();
        }

        private void LoadScene(SceneEnum sceneEnum)
        {
            asyncSceneLoading = SceneManager.LoadSceneAsync(sceneEnum.ToString(), LoadSceneMode.Additive);
            asyncSceneLoading.allowSceneActivation = false;

            if (ActiveScene != SceneEnum.Initial)
                _popupEffector.PlayTransitionSceneEffect(CompleteAnimation, SCENE_TRANSITION_DURATION);
            else
                CompleteAnimation();

            _loadedScene.Add(sceneEnum);
        }
        
        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= SceneLoaded;
            
            SceneEnum key = (SceneEnum) Enum.Parse(typeof(SceneEnum), scene.name);
            onLoadTransitions[key]?.Invoke();

            ActiveScene = _currentLoadedScene;
            
            OnSceneLoaded?.Invoke(key);
        }

        private void OnGameLoaded()
        {
            if (ActiveScene != SceneEnum.Map && ActiveScene != SceneEnum.Initial)
                SceneManager.UnloadSceneAsync(ActiveScene.ToString());

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLoadedScene.ToString()));
            
            OnSceneLoaded?.Invoke(SceneEnum.Game);
        }

        private void DefaultLoad()
        {
            LoadScene(_currentLoadedScene);
            
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void DefaultOnLoaded()
        {
            if (ActiveScene != SceneEnum.Initial)
                SceneManager.UnloadSceneAsync(ActiveScene.ToString());
            
            _loadedScene.Remove(ActiveScene);
            
            OnSceneUnloaded?.Invoke(ActiveScene);
        }

        private void CompleteAnimation()
        {
            asyncSceneLoading.allowSceneActivation = true;

            if (asyncSceneLoading.progress < 0.9f)
                _loadingPopup.ShowLoading(asyncSceneLoading);
        }
    }
}