using System.Collections.Generic;
using Core.Items;

namespace Core.Data
{
    public class ItemDataProvider: IStaticDataProvider
    {
        public List<ItemConfig> items;

        public ItemDataProvider(List<ItemConfig> items)
        {
            this.items = items;
        }
    }
}