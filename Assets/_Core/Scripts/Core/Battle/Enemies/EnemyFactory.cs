using Core.Data;
using Core.Effects;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public class EnemyFactory
    {
        [Inject] private Player player;
        [Inject] private EffectManager effectManager;
        
        public EnemyBasePresenter Create(EnemyConfig config, Vector2 anchorePosition, Transform parent)
        {
            var view = Object.Instantiate(config.prefab, parent);
            view.RectTransform.anchoredPosition = anchorePosition;
            return new EnemyPresenter(view, config, player, effectManager);
        }
    }
}