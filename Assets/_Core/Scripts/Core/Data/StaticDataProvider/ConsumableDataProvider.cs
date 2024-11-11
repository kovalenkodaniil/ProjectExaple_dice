using System.Collections.Generic;
using Core.Data.Consumable;
using Object = UnityEngine.Object;

namespace Core.Data
{
    public class ConsumableDataProvider : IStaticDataProvider
    {
        public ConsumableAsset Asset { get; private set; }
        private Dictionary<EnumConsumable, ConsumableData> consumables = new();

        public ConsumableDataProvider(ConsumableAsset asset)
        {
            this.Asset = asset;
            
            foreach (var item in asset.Consumables)
                AddConsumable(item);
        }

        public ConsumableData GetConsumable(EnumConsumable type)
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
        }
    }
}