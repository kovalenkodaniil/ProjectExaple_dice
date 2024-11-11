using Core.Data;
using Core.Effects;
using PlayerScripts;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public class EnemyPresenter : EnemyBasePresenter
    {
        public EnemyPresenter(EnemyBase view, EnemyConfig config, Player player, EffectManager effectManager) : base(view, config, player, effectManager) { }
    }
}