using System.Collections.Generic;
using UnityEngine;

namespace Core.PreBattle
{
    public interface ITabGroup
    {
        public List<ITab> Tabs { get; set; }

        public void SelectTabs(ITab target);
    }
}