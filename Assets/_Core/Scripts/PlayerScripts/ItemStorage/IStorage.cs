using System;
using System.Collections.Generic;
using System.Linq;
using R3;

public interface IStorage<T>
{
    event Action<T, int> OnChanged; 
    int Count(T type);
    int CountAll();
    void Add(T type, int amount);
    bool TrySpend(T type, int amount);
    void Set(T type, int amount);
    ReadOnlyReactiveProperty<int> GetItem(T type);
    void ClearAll();
    IEnumerable<KeyValuePair<T, ReadOnlyReactiveProperty<int>>> GetAllItems();
}