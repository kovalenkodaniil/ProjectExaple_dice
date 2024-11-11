using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data.Consumable;
using R3;

public class ConsumableStorage : IStorage<EnumConsumable>
{
    public event Action<EnumConsumable, int> OnChanged;

    private Dictionary<EnumConsumable, ReactiveProperty<int>> items = new();

    public ConsumableStorage()
    {
        foreach (EnumConsumable value in Enum.GetValues(typeof(EnumConsumable)))
            items.Add(value, new(0));
    }

    public int Count(EnumConsumable type)
    {
        return items.GetValueOrDefault(type).Value;
    }

    public bool Contains(EnumConsumable type)
    {
        return items[type].Value > 0;
    }

    public int CountAll()
    {
        return items.Sum(x => x.Value.Value);
    }

    public void Add(EnumConsumable type, int amount = 1)
    {
        if (amount > 0)
        {
            var currentItem = items[type];
            currentItem.Value += amount;
            OnChanged?.Invoke(type, currentItem.Value);
        }
    }

    public bool TrySpend(EnumConsumable type, int amount = 1)
    {
        var currentItem = items[type];

        if (amount > currentItem.Value)
            return false;

        currentItem.Value -= amount;
        OnChanged?.Invoke(type, currentItem.Value);
        return true;
    }

    public void Set(EnumConsumable type, int amount)
    {
        if (amount >= 0)
        {
            var currentItem = items[type];
            currentItem.Value = amount;
            OnChanged?.Invoke(type, currentItem.Value);
        }
    }

    public ReadOnlyReactiveProperty<int> GetItem(EnumConsumable type)
    {
        return items.GetValueOrDefault(type);
    }

    public void ClearAll()
    {
        foreach (var kvp in items)
            kvp.Value.Value = 0;
    }

    public IEnumerable<KeyValuePair<EnumConsumable, ReadOnlyReactiveProperty<int>>> GetAllItems()
    {
        return Enum.GetValues(typeof(EnumConsumable))
            .Cast<EnumConsumable>()
            .Where(currencyType => items.ContainsKey(currencyType))
            .Select(currencyType => new KeyValuePair<EnumConsumable, ReadOnlyReactiveProperty<int>>(currencyType, items[currencyType]));
    }
}