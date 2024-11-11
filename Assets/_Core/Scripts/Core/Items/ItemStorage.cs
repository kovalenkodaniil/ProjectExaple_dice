using System.Collections.Generic;
using Core.Data;

namespace Core.Items
{
    public class ItemStorage
    {
        public List<ItemConfig> itemInBattle;
        public List<ItemConfig> availableItem;

        public ItemStorage()
        {
            itemInBattle = new List<ItemConfig>();
            availableItem = new List<ItemConfig>();
            
            availableItem.AddRange(StaticDataProvider.Get<ItemDataProvider>().items);
            itemInBattle.AddRange(StaticDataProvider.Get<ItemDataProvider>().items);
        }
        
        public void ReplaceItemInBattle(ItemConfig oldItem, ItemConfig newItem)
        {
            itemInBattle.Remove(oldItem);
            itemInBattle.Add(newItem);
        }
    }
}