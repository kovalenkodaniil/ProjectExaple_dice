using System;
using System.Collections.Generic;
using System.Linq;
using PlayerScripts;
using R3;

public class CurrencyStorage : IStorage<EnumCurrency>
{
    public event Action<EnumCurrency, int> OnChanged;

    private Dictionary<EnumCurrency, ReactiveProperty<int>> items = new();

    public CurrencyStorage()
    {
        foreach (EnumCurrency value in Enum.GetValues(typeof(EnumCurrency)))
            items.Add(value, new(0));
    }

    public int Count(EnumCurrency type)
    {
        return items.GetValueOrDefault(type).Value;
    }

    public int CountAll()
    {
        return items.Sum(x => x.Value.Value);
    }

    public void Add(EnumCurrency type, int amount)
    {
        if (amount > 0)
        {
            var currentItem = items[type];
            currentItem.Value += amount;
            OnChanged?.Invoke(type, currentItem.Value);
        }
    }

    public bool TrySpend(EnumCurrency type, int amount)
    {
        var currentItem = items[type];

        if (amount > currentItem.Value)
            return false;

        currentItem.Value -= amount;
        OnChanged?.Invoke(type, currentItem.Value);
        return true;
    }

    public void Set(EnumCurrency type, int amount)
    {
        if (amount >= 0)
        {
            var currentItem = items[type];
            currentItem.Value = amount;
            OnChanged?.Invoke(type, currentItem.Value);
        }
    }

    public ReadOnlyReactiveProperty<int> GetItem(EnumCurrency type)
    {
        return items.GetValueOrDefault(type);
    }

    public void ClearAll()
    {
        foreach (var kvp in items)
            kvp.Value.Value = 0;
    }

    public IEnumerable<KeyValuePair<EnumCurrency, ReadOnlyReactiveProperty<int>>> GetAllItems()
    {
        return Enum.GetValues(typeof(EnumCurrency))
            .Cast<EnumCurrency>()
            .Where(currencyType => items.ContainsKey(currencyType))
            .Select(currencyType => new KeyValuePair<EnumCurrency, ReadOnlyReactiveProperty<int>>(currencyType, items[currencyType]));
    }
}