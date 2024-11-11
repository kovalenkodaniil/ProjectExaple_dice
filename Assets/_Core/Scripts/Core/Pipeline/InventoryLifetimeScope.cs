using _Core.Scripts.Core.Data;
using Core.Data;
using Core.InventoryScripts;
using Managers;
using PlayerScripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utils.Pipeline
{
    public class InventoryLifetimeScope : LifetimeScope
    {
        [Header("StaticData")]
        [SerializeField] private GameDataConfig _gameStaticData;
        
        [Header("Popups")]
        [SerializeField] private InventoryPopup _inventoryPopup;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Player>(Lifetime.Singleton);

            builder.RegisterComponent(_inventoryPopup);

            builder.RegisterInstance(_gameStaticData);

            builder.RegisterEntryPoint<EntryPointInventory>();
        }
    }
}