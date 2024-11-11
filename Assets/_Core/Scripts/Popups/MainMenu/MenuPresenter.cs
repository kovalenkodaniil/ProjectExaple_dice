using Core.Data;
using Core.Features.Talents.Scripts;
using Core.InventoryScripts;
using Managers;
using Popups;
using VContainer;

public class MenuPresenter
{
    [Inject] private MenuPopup menuView;
    [Inject] private GameStats gameStats;
    [Inject] private TalentsView talents;
    [Inject] private PopupManager inventory;
    
    public void OnClickBattle()
    {
        if (StaticDataProvider.Get<BattleDataProvider>().AllBattles == gameStats.BattlesCounter)
        {
            return;
        }
        
        SceneLoader.Instance.Load(SceneEnum.Game);
    }

    public void OnClickUpgrade()
    {
        talents.Open();
    }
    
    public void OnClickInventory()
    {
        inventory._inventoryPopup.Open();
    }
}