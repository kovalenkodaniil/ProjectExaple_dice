using _Core.Scripts.Core.Battle.Enemies;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace Core.Items.ItemEffects
{
    public class TheBronzeHorseshoeEffect: ItemEffect
    {
        [Inject] private EnemyBattleManager _enemyBattleManager;
        [Inject] private Player _player;
        
        public override void Initialize(ItemConfig config)
        {
            base.Initialize(config);

            _enemyBattleManager.Enemies.ForEach(enemy =>
            {
                enemy.model.OnTakeOverDamage += Apply;
            });
        }

        public void Apply(int overDamage, CombatEntity model)
        {
            base.Apply();

            model.OnTakeOverDamage -= Apply;
            
            if (_enemyBattleManager.Enemies.Count > 0)
                _enemyBattleManager.Enemies[Random.Range(0, _enemyBattleManager.Enemies.Count)].model.TakeDamage(overDamage);
        }

        public override void Reset()
        {
            _enemyBattleManager.Enemies.ForEach(enemy =>
            {
                enemy.model.OnTakeOverDamage -= Apply;
            });
        }
    }
}