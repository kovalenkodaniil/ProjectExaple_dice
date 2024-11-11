using _Core.Scripts.Core.Battle.Combinations;
using _Core.Scripts.Core.Battle.Dice;
using _Core.Scripts.Core.Battle.Enemies;
using _Core.Scripts.Core.Battle.SkillScripts;
using _Core.Scripts.Core.Data;
using _Core.Scripts.Popups;
using Core.Data;
using Core.Items;
using Core.PreBattle;
using Managers;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace _Core.Scripts.Core.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [Inject] private Player _player;
        [Inject] private SoundManager _soundManager;
        [Inject] private GameDataConfig _gameStaticData;
        [Inject] private DiceTower _diceTower;
        [Inject] private PopupLose _popupLose;
        [Inject] private PopupManager _popupManager;
        [Inject] private TurnManager _turnManager;
        [Inject] private SkillCaster _skillCaster;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private BattleUIPresenter _battleUIPresenter;
        [Inject] private BattleVFXEffector _battleVFXEffector;
        [Inject] private BattlePreparation _battlePreparePopup;
        [Inject] private IObjectResolver _injectionInstantiator;
        [Inject] private EnemyBattleManager enemyBattleManager;
        [Inject] private CombinationResolver _combinationResolver;
        [Inject] private GameStats gameStats;
        [Inject] private EndGameHandler endGameHandler;
        [Inject] private ItemManager itemManager;
        
        [SerializeField] private GameObject _battleContainer;
        
        private int _turnNumber;
        private bool _isBattleStart;

        public void Initialize()
        {
            SetupTurnManager();
            SetupDiceTower();
            SetupSkillCaster();
            SetupPlayer();
            SetupBattleUI();
            
            SetupEndGameHandler();

            SetupEnemyManager();
            
            _combinationResolver.Initialize();
            itemManager.Initialize();

            _popupLose.OnBattleRetry += RetryBattle;
        }

        public void Start()
        {
            if (_isBattleStart) return;
            
            _battlePreparePopup.OnBattleReady += StartBattle;
            _battlePreparePopup.Open();
        }

        private void FinishBattle()
        {
            enemyBattleManager.FinishBattle();
            _diceTower.FinishBattle();
            _combinationResolver.Reset();
            itemManager.FinishBattle();
            _skillCaster.FinishBattle();
            
            UnSub();
        }

        private void RetryBattle()
        {
            _popupLose.OnBattleRetry -= RetryBattle;
            _battlePreparePopup.OnBattleReady += StartBattle;

            _battlePreparePopup.Open();
        }

        private void StartBattle()
        {
            _battlePreparePopup.OnBattleReady -= StartBattle;
            
            _battleContainer.gameObject.SetActive(true);
            
            Initialize();
            
            _soundManager.PauseTheme();
            _soundManager.PlayTheme(_soundManager.SoundList.battleTheme);
        }

        private void OnDestroy()
        {
            FinishBattle();
        }

        private void UnSub()
        {
            _turnManager.OnStartTurn -= StartTurn;
            _turnManager.OnRollDice -= _diceTower.RollDice;
            _turnManager.OnRollResult -= _diceTower.ResolveDice;
            _turnManager.OnPlayerAttack -= _player.Attack;
            _turnManager.OnEndTurn -= EndTurn;
            _turnManager.OnRollDice -= _battleUIPresenter.DisableRoll;
            _turnManager.OnRollDice -= _battleUIPresenter.EnableEndTurn;
            _turnManager.OnStartTurn -= _battleUIPresenter.EnableRoll;
            _turnManager.OnStartTurn -= _battleUIPresenter.DisableEndTurn;
            _turnManager.OnRollResult -= _skillCaster.DisableSpells;
            _turnManager.OnRollDice -= _skillCaster.EnableSpells;
            
            _player.OnPlayerAttack -= _turnManager.NextState;
            
            _diceTower.OnRollResolved -= _turnManager.NextState;
            
            _battleUIPresenter.OnRollButtonClicked -= _turnManager.NextState;
            _battleUIPresenter.OnEndTurnButtonClicked -= _turnManager.NextState;
            
            endGameHandler.OnGameEnded -= FinishBattle;
        }

        private void SetupEndGameHandler()
        {
            endGameHandler.Init();
            endGameHandler.OnGameEnded += FinishBattle;
        }
        
        private void SetupEnemyManager()
        {
            enemyBattleManager.Init(StaticDataProvider.Get<BattleDataProvider>().GetBattleData(gameStats.BattlesCounter));
            _player.SetEnemyManager(enemyBattleManager);
        }

        private void SetupTurnManager()
        {
            _turnManager.OnStartTurn += StartTurn;
            _turnManager.OnStartTurn += _battleUIPresenter.EnableRoll;
            _turnManager.OnStartTurn += _battleUIPresenter.DisableEndTurn;
            
            _turnManager.OnRollDice += _diceTower.RollDice;
            _turnManager.OnRollDice += _skillCaster.EnableSpells;
            _turnManager.OnRollDice += _battleUIPresenter.DisableRoll;
            _turnManager.OnRollDice += _battleUIPresenter.EnableEndTurn;

            _turnManager.OnRollResult += _diceTower.ResolveDice;
            _turnManager.OnRollResult += _skillCaster.DisableSpells;

            _turnManager.OnPlayerAttack += _player.Attack;

            _turnManager.OnEndTurn += EndTurn;
            
            _turnManager.Initialize();
        }

        private void SetupPlayer()
        {
            _player.Reset();

            _player.OnPlayerAttack += _turnManager.NextState;
        }

        private void SetupDiceTower()
        {
            _diceTower.OnRollResolved += _turnManager.NextState;
            
            _diceTower.Initialize();
        }

        private void SetupSkillCaster()
        {
            _skillCaster.Initialize();
        }

        private void SetupBattleUI()
        {
            _battleUIPresenter.InitializeUI();
            _battleUIPresenter.SetPlayerCounters();

            _battleUIPresenter.OnRollButtonClicked += _turnManager.NextState;
            _battleUIPresenter.OnEndTurnButtonClicked += _turnManager.NextState;
        }

        private void StartTurn()
        {
            _player.ResetRound();
            _diceTower.PrepareNewTurn();
        }

        private void EndTurn()
        {
            _battleVFXEffector.OffEnemyStunEffect();
            _turnManager.NextState();
        }
    }
}