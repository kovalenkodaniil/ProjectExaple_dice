using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Enemies;
using _Core.Scripts.Core.Battle.SkillScripts;
using _Core.Scripts.Core.Battle.Stat;
using _Core.Scripts.Core.Data;
using Core.Data;
using Core.Features.Talents.Scripts;
using Core.Items;
using Managers;
using UnityEngine;
using VContainer;

namespace PlayerScripts
{
    public class Player
    {
        [Inject] private GameDataConfig _gameStaticData;
        [Inject] private GameStats stats;
        [Inject] private TalentManager talents;

        public HealthComponent HealthComponent { get; private set; }
        public ArmorComponent ArmorComponent { get; private set; }
        public ManaComponent ManaComponent { get; private set; }
        public DamageComponent DamageComponent { get; private set; }
        public KarmaComponent KarmaComponent { get; private set; }
        public FixationComponent FixationComponent { get; private set; }

        public bool isConnectingActive;
        public bool isStuned;
        public string name;
        public SkillStorage skillStorage;
        public List<DiceData> availableDice;
        public List<DiceData> diceInBattle;

        public PlayerModel model { get; private set; }
        public CurrencyStorage currencyStorage = new();
        public ConsumableStorage consumableStorage = new();
        public ItemStorage itemStorage;
        public InBattleConsumablesService inBattleConsumablesService { get; private set; }
        private PlayerStaticData _playerData;
        private EnemyBattleManager enemyBattleManager;
        private ConsumableDataProvider consumableProvider;

        public event Action OnPlayerAttack;

        private void BeforeInitialize()
        {
            HealthComponent = new HealthComponent(talents);
            ArmorComponent = new ArmorComponent();
            ManaComponent = new ManaComponent(talents);
            DamageComponent = new DamageComponent();
            KarmaComponent = new KarmaComponent();
            FixationComponent = new FixationComponent(talents);

            skillStorage = new SkillStorage();
            itemStorage = new ItemStorage();

            availableDice = new List<DiceData>();
            diceInBattle = new List<DiceData>();

            inBattleConsumablesService = new(consumableStorage);
        }

        private void InitStorages(PlayerData data)
        {
            currencyStorage = data.currencyStorage;
            consumableStorage = data.consumableStorage;
        }

        public void ResetPlayerData()
        {
            BeforeInitialize();
            
            _playerData = _gameStaticData.playerData;
            
            stats.Reset();

            model = new();
            model.AddExpirience(_playerData.startExpirience);
            
            HealthComponent.MaxValue = _playerData.maxHealth;
            ManaComponent.MaxValue = _playerData.startMana;
            FixationComponent.MaxValue = _playerData.startFixation;
            ManaComponent.AddMana(_playerData.startMana);

            inBattleConsumablesService.Clear();

            currencyStorage.ClearAll();
            consumableStorage.ClearAll();
            inBattleConsumablesService.Clear();
            
            diceInBattle.Clear();
            availableDice.Clear();

            _gameStaticData.playerData.startAvailableDice.ForEach(dice =>
            {
                DiceData diceData = new DiceData(dice.config, dice.quantity);
                
                availableDice.Add(diceData);
                
                if (_gameStaticData.playerData.startDices.Find(config => config.id == dice.config.id))
                    diceInBattle.Add(diceData);
            });
        }

        public void RemoveDiceFromBattle(DiceData diceData)
        {
            diceInBattle.Remove(diceData);
            
            diceData.RemoveFromBattle();
        }

        public void AddDiceInBattle(DiceData diceData)
        {
            diceInBattle.Add(diceData);
            
            diceData.AddInBattle();
        }

        public void AddDamage(int effectValue)
        {
            DamageComponent.AddDamage(effectValue);
        }

        public void TakeDamage(int value, EnemyBasePresenter enemy)
        {
            HealthComponent.TakeDamage(ArmorComponent.GetDamageAfterBlock(value));

            if (isConnectingActive) NonTurnAttack(value, enemy);
        }

        public void NonTurnAttack(int value, EnemyBasePresenter enemy)
        {
            if (value <= 0) return;
            
            enemy.model.TakeDamage(value);
        }

        public void Attack()
        {
            if (DamageComponent.Damage <= 0)
            {
                OnPlayerAttack?.Invoke();
                return;
            }

            enemyBattleManager.PlayOutline(true);
            CoroutineManager.StartCoroutine(WaitAttack());

            return;

            IEnumerator WaitAttack()
            {
                while (true)
                {
                    if (Input.GetMouseButton(0))
                    {
                        var mousePos = Input.mousePosition;

                        if (enemyBattleManager.IsMouseOverEnemy(mousePos, out var combatEntity))
                        {
                            combatEntity.TakeDamage(DamageComponent.Damage);
                            DamageComponent.Reset();
                            enemyBattleManager.PlayOutline(false);
                            OnPlayerAttack?.Invoke();
                            yield break;   
                        }
                    }

                    yield return null;
                }                
            }
        }

        public void AddArmor(int value) => ArmorComponent.AddArmor(value);

        public void AddMana(int value) => ManaComponent.AddMana(value);

        public void TakeNoLethalDamage(int value) => HealthComponent.TakeNoLethalDamage(value);

        public void ResetRound()
        {
            ArmorComponent.ResetArmor();
            ArmorComponent.ResetArmorBuf();
            
            DamageComponent.Reset();
            
            isConnectingActive = false;
        }

        public void SetEnemyManager(EnemyBattleManager enemyBattleManager)
        {
            this.enemyBattleManager = enemyBattleManager;
        }

        public void Reset()
        {
            HealthComponent.Reset();
            ManaComponent.mana = 0;
            ArmorComponent.ResetArmor();
            isConnectingActive = false;
        }

        public void AddBattleRewards(BattleData battleData)
        {
            model.AddExpirience(battleData.Expirience);
            AddLoot(battleData.Loot);
        }

        private void AddLoot(List<Loot> lootList)
        {
            foreach (var loot in lootList)
            {
                switch (loot)
                {
                    case LootConsumable cons:
                        consumableStorage.Add(cons.consumableType, loot.quantity);
                        break;
                    case LootCurrency curr:
                        currencyStorage.Add(curr.currencyType, curr.quantity);
                        break;
                }
            }
        }
    }
}