using System;
using Core.InventoryScripts;
using PlayerScripts;

namespace Core.PreBattle
{
    public interface ITab
    {
        public void Initialize(Player _player);
        public void Open();
        public void Close();

        public event Action OnOpen;
        public event Action<ITab> OnClose;
    }
}