using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.Data.Consumable;

public class InBattleConsumablesService
{
    public List<ConsumableData> consumablesInBattle { get; private set; }

    private readonly int inBattleLimit;
    private readonly ConsumableDataProvider provider;
    private readonly ConsumableStorage consumableStorage;

    public InBattleConsumablesService(ConsumableStorage consumableStorage)
    {
        this.consumableStorage = consumableStorage;
        
        provider = StaticDataProvider.Get<ConsumableDataProvider>();
        inBattleLimit = provider.Asset.MaxConsumables;

        consumablesInBattle = new(inBattleLimit);
        
        consumableStorage.OnChanged += OnConsumablesChanged;
        
        TryToGetMore();
    }

    public int InBattleCount => consumablesInBattle.Count;
    public bool IsFull => consumablesInBattle.Count >= inBattleLimit;

    ~InBattleConsumablesService()
    {
        consumableStorage.OnChanged -= OnConsumablesChanged;
    }

    public void Add(ConsumableData data)
    {
        consumablesInBattle.Add(data);
    }

    public void Replace(int index, ConsumableData data)
    {
        consumablesInBattle[index] = data;
    }

    public void Clear()
    {
        consumablesInBattle.Clear();
    }

    private void OnConsumablesChanged(EnumConsumable type, int newValue)
    {
        var typesInBattleCount = consumablesInBattle.Count(x => x.ConsumableType == type);

        if (typesInBattleCount < newValue && consumablesInBattle.Count < inBattleLimit)
        {
            Add(provider.GetConsumable(type));
        }
        else if (typesInBattleCount > newValue)
        {
            var inBattleConsumIndex = consumablesInBattle.FindIndex(x => x.ConsumableType == type);
    
            if (inBattleConsumIndex >= 0)
                consumablesInBattle.RemoveAt(inBattleConsumIndex);
        }

        TryToGetMore();
    }

    private void TryToGetMore()
    {
        if (consumablesInBattle.Count >= inBattleLimit)
            return;

        int needToAdd = inBattleLimit - consumablesInBattle.Count;
        
        foreach (var kvp in consumableStorage.GetAllItems())
        {
            var inStorageAmount = kvp.Value.CurrentValue;
            var inBattleAmount = consumablesInBattle.Count(x => x.ConsumableType == kvp.Key); 
            
            if (inStorageAmount > inBattleAmount)
            {
                int availableToAdd = Math.Min(inStorageAmount - inBattleAmount, needToAdd);

                for (int i = 0; i < availableToAdd; i++)
                    consumablesInBattle.Add(provider.GetConsumable(kvp.Key));

                needToAdd -= availableToAdd;

                if (needToAdd <= 0)
                    return;
            }
        }
    }

    public void Set(List<ConsumableData> list)
    {
        if (list is { Count: > 0 })
        {
            consumablesInBattle = list;
        }
    }
}