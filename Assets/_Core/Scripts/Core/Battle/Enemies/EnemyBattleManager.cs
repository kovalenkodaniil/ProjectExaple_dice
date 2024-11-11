using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Core.Scripts.Core.Greyness;
using Core.Data;
using Core.Effects;
using Managers;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public class EnemyBattleManager : MonoBehaviour
    {
        [Inject] private EnemyFactory enemyFactory;
        [Inject] private Player player;
        [Inject] private TurnManager turnManager;
        [Inject] private GreynessManager greynessManager;
        [Inject] private EffectManager effectManager;

        public event Action OnEnemiesKilled;

        float xOffsetCreate = 425f;
        float yOffsetCreate = -8f;
        private int turnCounter;
        private BattleData battleData;
        public List<EnemyBasePresenter> Enemies { get; private set; } = new(3);

        [SerializeField] private RectTransform spawnMarker;

        public void Init(BattleData battleData)
        {
            this.battleData = battleData;

            turnManager.OnEnemyAttack += HandlerOnEnemyAttack;
            turnManager.OnStartTurn += HandlerOnStartTurn;

            CreateEnemies(battleData.Enemies);
            SetupStartBuffs();
            HandlerOnStartTurn();
        }

        public void OnDisable()
        {
            FinishBattle();
        }

        public void FinishBattle()
        {
            foreach (var pres in Enemies)
                pres.Destroy();
            
            Enemies.Clear();

            turnManager.OnEnemyAttack -= HandlerOnEnemyAttack;
            turnManager.OnStartTurn -= HandlerOnStartTurn;
        }

        public void PlayOutline(bool value)
        {
            GlobalCamera.Instance.SetActiveBloom(value);
            foreach (var enemy in Enemies)
                enemy.view.PlayOutline(value);
        }

        public bool IsMouseOverEnemy(Vector2 mousePos, out CombatEntity combatEntity)
        {
            combatEntity = Enemies.FirstOrDefault(x => x.view.IsMouseOverEnemy(mousePos))?.model;
            return combatEntity != null;
        }
        
        public bool IsMouseOverIntention(Vector3 mousePos, out EnemyModel combatEntity, out int intentionIndex, out Vector3 targetPostion)
        {
            intentionIndex = 0;
            combatEntity = null;
            targetPostion = Vector3.zero;
            
            foreach (var enemy in Enemies)
            {
                if (enemy.view.IsMouseOverIntention(mousePos, out int index))
                {
                    targetPostion = enemy.view.transform.position;
                    intentionIndex = index;
                    combatEntity = enemy.model;
                    break;
                }
            }
            
            return combatEntity != null;
        }

        private void HandlerOnEnemyAttack()
        {
            StartCoroutine(EnemiesTurn());

            return;

            IEnumerator EnemiesTurn()
            {
                foreach (var enemy in Enemies)
                {
                    enemy.StartTurn();
                    yield return new WaitUntil(() => enemy.isTurnCompleted);
                    yield return new WaitForSeconds(.5f);
                }

                turnManager.NextState();
            }
        }
        
        private void HandlerOnStartTurn()
        {
            turnCounter++;
            foreach (var enemy in Enemies)
                enemy.NewTurn(turnCounter);
        }

        private void CreateEnemies(List<EnemyConfig> list)
        {
            int enemyCounter = 0;

            foreach (var conf in list)
            {
                var enemy = enemyFactory.Create(conf, spawnMarker.position + new Vector3(xOffsetCreate * enemyCounter, -13.8f), transform);

                if (enemyCounter > 0)
                    enemy.view.transform.localScale = Vector3.one * 0.92f;
                
                enemy.view.transform.SetAsFirstSibling();
                Enemies.Add(enemy);
                enemy.OnDie += HandlerOnEnemyDie;
                enemyCounter++;
            }
        }

        private void SetupStartBuffs()
        {
            greynessManager.GetCurrentEffects().ForEach(effect =>
            {
                if (effect != null)
                {
                    foreach (var enemy in Enemies)
                    {
                        enemy.model.AddEffect(effectManager.GetEffect(effect.EffectType),effect.Value, -1);
                    }
                }
            });
        }

        private void HandlerOnEnemyDie(EnemyBasePresenter enemyPres)
        {
            int index = Enemies.IndexOf(enemyPres);
            
            if (index >= 0)
            {
                for (int i = index + 1; i < Enemies.Count; i++)
                {
                    var enemyPresenter = Enemies[i];

                    float yOffset = index == 0 ? 0 : yOffsetCreate;
                    enemyPresenter.view.RectTransform.anchoredPosition -= new Vector2(xOffsetCreate * i, yOffset);

                    if (index == 0)
                    {
                        enemyPresenter.view.transform.localScale = Vector3.one;   
                    }
                }
            }
            
            Enemies.Remove(enemyPres);
            enemyPres.OnDie -= HandlerOnEnemyDie;
            enemyPres.Destroy();

            if (Enemies.Count == 0)
                OnEnemiesKilled?.Invoke();
        }
    }
}