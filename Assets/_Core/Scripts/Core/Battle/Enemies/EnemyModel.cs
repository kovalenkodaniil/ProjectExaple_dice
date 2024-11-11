using System;
using Core.Data;
using Core.Effects;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public class EnemyModel : EnemyCombatEntity
    {
        private EnemyConfig data;
        private EffectManager effectManager;
        
        public EnemyModel(EnemyConfig config, EffectManager effectManager)
        {
            this.effectManager = effectManager;
            this.data = config;
            BaseHealth = Health = config.health;
        }

        public bool IsHaveIntentions => QueueIntentions.Count > 0;

        public void ExecuteIntention(Action callback)
        {
            if (IsStuned)
            {
                callback?.Invoke();
                return;   
            }
            
            var intention = QueueIntentions.Dequeue();

            if (intention.IsStuned)
            {
                intention.IsStuned = false;
                callback?.Invoke();
                return;
            }

            foreach (var effect in intention.action.effects)
            {
                var ef = effectManager.GetEffect(effect);
                AddEffect(ef, intention.power, intention.Duration);
            }
            
            callback?.Invoke();
        }

        public void UpdateIntentions(int turnNumber)
        {
            var intentions = data.intentions;
            QueueIntentions.Clear();
            foreach (var enemyTurn in intentions[(turnNumber - 1) % intentions.Count].Turns)
                QueueIntentions.Enqueue(enemyTurn);
        }
    }
}