using _Core.Scripts.Core.Battle;
using _Core.Scripts.Core.Battle.Combinations;
using _Core.Scripts.Core.Battle.Dice;
using _Core.Scripts.Core.Battle.Enemies;
using _Core.Scripts.Core.Battle.SkillScripts;
using _Core.Scripts.Popups;
using Core.Items;
using Core.PreBattle;
using Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utils.Pipeline
{
    public class BattleLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnemyBattleManager enemyManager;
        [SerializeField] private BattleUIPresenter _battleUIPresenter;
        [SerializeField] private TurnManager _turnManager;
        [SerializeField] private DiceTower _diceTower;
        [SerializeField] private SkillCaster _skillCaster;
        [SerializeField] private BattlePreparation _battlePreparation;
        [SerializeField] private PopupLose _popupLose;
        [SerializeField] private BattleManager _battleManager;
        [SerializeField] private BattleVFXEffector _battleVFXEffector;
        [SerializeField] private CombinationResoverView _combinationResoverView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_battleUIPresenter);
            builder.RegisterComponent(_turnManager);
            builder.RegisterComponent(_diceTower);
            builder.RegisterComponent(_skillCaster);
            builder.RegisterComponent(_battlePreparation);
            builder.RegisterComponent(_popupLose);
            builder.RegisterComponent(_battleManager);
            builder.RegisterComponent(_battleVFXEffector);
            builder.RegisterComponent(_combinationResoverView);

            builder.RegisterComponent(enemyManager);

            builder.Register<CombinationResolver>(Lifetime.Singleton);
            builder.Register<EnemyFactory>(Lifetime.Singleton);
            builder.Register<EndGameHandler>(Lifetime.Singleton);
            builder.Register<ItemManager>(Lifetime.Singleton);
        }
    }
}