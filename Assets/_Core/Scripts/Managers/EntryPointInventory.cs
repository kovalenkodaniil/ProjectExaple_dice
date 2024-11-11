using Core.InventoryScripts;
using PlayerScripts;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public class EntryPointInventory : IStartable
    {
        [Inject] private Player _player;
        [Inject] private InventoryPopup _inventoryPopup;
        
        public void Start()
        {
            _player.ResetPlayerData();
            
            _inventoryPopup.Open();
        }
    }
}