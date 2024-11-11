using System;
using System.Collections;
using Core.Data;
using Core.Effects;
using Managers;
using PlayerScripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public abstract class EnemyBasePresenter
    {
        public event Action<EnemyBasePresenter> OnDie;

        public bool isTurnCompleted { get; private set; }
        public EnemyModel model { get; private set; }
        public EnemyBase view { get; private set; }
        
        private Player player;
        private Coroutine turnCoroutine;

        public EnemyBasePresenter(EnemyBase view, EnemyConfig config, Player player, EffectManager effectManager)
        {
            this.player = player;
            this.view = view;

            model = new EnemyModel(config, effectManager);
            model.OnDie += Model_OnDie;
            model.OnHeal += Model_OnHeal;
            model.OnShieldChanged += View_UpdateArmor;
            model.OnTakeDamage += Model_OnTakeDamage;
            model.OnDealDamage += Model_OnDealDamage;
            model.OnTurnEnd += Model_OnTurnEnd;

            view.Image = config.sprite;
            View_UpdateHealth(model.Health);
            View_UpdateArmor(0);
        }

        public void Destroy()
        {
            model.OnDie -= Model_OnDie;
            model.OnHeal -= Model_OnHeal;
            model.OnShieldChanged -= View_UpdateArmor;
            model.OnTakeDamage -= Model_OnTakeDamage;
            model.OnDealDamage -= Model_OnDealDamage;
            model.OnTurnEnd -= Model_OnTurnEnd;
            Object.Destroy(view.gameObject);
            
            if (turnCoroutine != null)
                CoroutineManager.StopCoroutine(turnCoroutine);
        }

        private void View_UpdateHealth(int health)
        {
            view.SetHealth(health / (float)model.BaseHealth, 1);
            view.HealthText = $"{health}/{model.BaseHealth}";
        }
        
        private void View_UpdateArmor(int armor)
        {
            view.SetArmor(armor);
            view.ArmorText = $"{armor}";
        }

        public void StartTurn()
        {
            isTurnCompleted = false;
            turnCoroutine = CoroutineManager.StartCoroutine(TurnRoutine());
        }

        public void NewTurn(int turnNumber)
        {
            model.UpdateIntentions(turnNumber);
            view.ShowIntentions(model.QueueIntentions);
        }
        
        private IEnumerator TurnRoutine()
        {
            model.TurnStart();
            
            while (model.IsHaveIntentions)
            {
                bool isExecuted = default;
                model.ExecuteIntention(() => isExecuted = true);
                yield return new WaitUntil(() => isExecuted);
                view.HideFirstIntention();
                yield return new WaitForSeconds(0.5f);   
            }

            model.TurnEnd();
        }
        
        private void Model_OnTurnEnd()
        {
            isTurnCompleted = true;
            view.SetActiveIntentions(false);
        }

        private void Model_OnDealDamage(int damage)
        {
            player.TakeDamage(damage, this);
        }

        private void Model_OnHeal()
        {
            View_UpdateHealth(model.Health);
        }

        private void Model_OnTakeDamage(int damage)
        {
            View_UpdateHealth(model.Health);
        }

        private void Model_OnDie()
        {
            OnDie?.Invoke(this);
        }
    }
}