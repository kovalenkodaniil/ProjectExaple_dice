using System.Collections.Generic;
using Core.Data.Consumable;
using UnityEngine;

namespace Core.Data
{
    public class BattleDataProvider : IStaticDataProvider
    {
        public EnemyAsset Asset { get; private set; }
        private List<BattleData> battles = new(20);
        private List<EnemyConfig> enemies = new(20);

        public int AllBattles => battles.Count;

        public BattleDataProvider(List<BattleData> battleData, EnemyAsset asset)
        {
            this.Asset = asset;
            
            foreach (var battle in battleData)
                AddBattleData(battle);
            
            /*foreach (var enemy in asset.enemies)
                AddEnemy(enemy);*/
        }

        public BattleData GetBattleData(int index)
        {
            return battles[index];
        }

        /*public ConsumableData GetConsumable(EnumConsumable type)
        {
            consumables.TryGetValue(type, out ConsumableData obj);
            return obj;
        }

        public bool TryGetConsumable(EnumConsumable type, out ConsumableData obj)
        {
            return consumables.TryGetValue(type, out obj);
        }

        public void ReplaceConsumable(ConsumableData obj)
        {
#if UNITY_EDITOR
            obj = Object.Instantiate(obj);
#endif
            consumables[obj.ConsumableType] = obj;
        }

        public void AddConsumable(ConsumableData obj)
        {
#if UNITY_EDITOR
            obj = Object.Instantiate(obj);
#endif
            consumables[obj.ConsumableType] = obj;
        }*/
        
        public void AddBattleData(BattleData obj)
        {
#if UNITY_EDITOR
            obj = Object.Instantiate(obj);
#endif
            battles.Add(obj);
        }
        
        public void AddEnemy(EnemyConfig obj)
        {
#if UNITY_EDITOR
            obj = Object.Instantiate(obj);
#endif
            enemies.Add(obj);
        }
    }
}