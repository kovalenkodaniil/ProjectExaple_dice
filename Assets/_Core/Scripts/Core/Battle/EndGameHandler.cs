using System;
using System.Collections;
using _Core.Scripts.Core.Battle.Enemies;
using _Core.Scripts.Core.Greyness;
using _Core.Scripts.Popups;
using _Core.Scripts.Popups.PopupRewards;
using Core.Data;
using DG.Tweening;
using Managers;
using PlayerScripts;
using Popups;
using Popups.PopupReward;
using UnityEngine;
using VContainer;

namespace _Core.Scripts.Core.Battle
{
    public class EndGameHandler
    {
        [Inject] private Player player;
        [Inject] private GameStats gameStats;
        [Inject] private SceneLoader sceneLoader;
        [Inject] private BattleUIPresenter battleUIPresenter;
        [Inject] private BattleVFXEffector battleVFXEffector;
        [Inject] private SoundManager soundManager;
        [Inject] private EnemyBattleManager enemyManager;
        [Inject] private PopupLose popupLose;
        [Inject] private PopupReward popupReward;
        [Inject] private GreynessManager _greynessManager;

        public event Action OnGameEnded;
    
        private Coroutine musicCoroutine;
    
        public void Init()
        {
            Subscribe();
        }

        private void OnPlayerDied()
        {
            Transit(false);
        }

        private void OnEnemyKilled()
        {
            battleUIPresenter.SetActiveAnticlick(true);
            Transit(true);
        }

        private void Transit(bool isWin)
        {
            if (isWin)
            {
                OnWin();
            }
            else
            {
                OnLose();
            }
        
            OnGameEnded?.Invoke();
        }
    
        private void OnWin()
        {
            Unsubscribe();
        
            var battleData = StaticDataProvider.Get<BattleDataProvider>().GetBattleData(gameStats.BattlesCounter);
            player.AddBattleRewards(battleData);
            gameStats.BattlesCounter++;
            _greynessManager.data.Stage++;

            popupReward.ShowBattleReward(battleData);
        
            soundManager.PlayTheme(soundManager.SoundList.winTheme);
            musicCoroutine = CoroutineManager.Wait(4.25f, () =>
            {
                if (musicCoroutine != null)
                    CoroutineManager.StopCoroutine(musicCoroutine);
        
                soundManager.PlayTheme(soundManager.SoundList.mainTheme);
            });
        }

        private void OnLose()
        {
            Unsubscribe();
        
            popupLose.Open();
            soundManager.PlayTheme(soundManager.SoundList.LoseTheme);
        }

        private void ExitToMenu()
        {
            Unsubscribe();

            battleUIPresenter.SetActiveAnticlick(false);
        
            if (musicCoroutine != null)
                CoroutineManager.StopCoroutine(musicCoroutine);
        
            soundManager.PlayTheme(soundManager.SoundList.mainTheme);

            CoroutineManager.StartCoroutine(KillTwinsAndExitToMenu());
        }
    
        private IEnumerator KillTwinsAndExitToMenu()
        {
            DOTween.KillAll();
            yield return null;
            sceneLoader.Load(SceneEnum.Menu);
        }

        private void Subscribe()
        {
            player.HealthComponent.OnDied += OnPlayerDied;
            enemyManager.OnEnemiesKilled += OnEnemyKilled;
            PausePopup.OnClickMainMenu += ExitToMenu;
        }

        private void Unsubscribe()
        {
            player.HealthComponent.OnDied -= OnPlayerDied;
            enemyManager.OnEnemiesKilled -= OnEnemyKilled;
            PausePopup.OnClickMainMenu -= ExitToMenu;
        }
    }
}