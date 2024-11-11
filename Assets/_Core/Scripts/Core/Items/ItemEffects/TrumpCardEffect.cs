using _Core.Scripts.Core.Battle.Combinations;
using _Core.Scripts.Core.Battle.Enemies;
using PlayerScripts;
using VContainer;
using Random = UnityEngine.Random;

namespace Core.Items.ItemEffects
{
    public class TrumpCardEffect : ItemEffect
    {
        [Inject] private EnemyBattleManager _enemyBattleManager;
        [Inject] private CombinationResolver _combinationResolver;
        [Inject] private Player _player;
        
        public override void Initialize(ItemConfig config)
        {
            base.Initialize(config);
            
            _combinationResolver.OnCombinationResolved += Apply;
        }

        public override void Apply()
        {
            base.Apply();
            _enemyBattleManager.Enemies[Random.Range(0, _enemyBattleManager.Enemies.Count)].model.TakeDamage(value);
        }

        public override void Reset()
        {
            _combinationResolver.OnCombinationResolved -= Apply;
        }
    }
}