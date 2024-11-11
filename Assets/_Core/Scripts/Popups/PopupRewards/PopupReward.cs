using System.Collections;
using System.Collections.Generic;
using Core.Data;
using Core.Data.Consumable;
using DG.Tweening;
using Managers;
using PlayerScripts;
using Popups;
using Popups.PopupReward;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Core.Scripts.Popups.PopupRewards
{
    public class PopupReward : MonoBehaviour
    {
        [Inject] private PopupEffector popupEffector;
        [Inject] private Player player;
        [Inject] private SceneLoader sceneLoader;

        private ConsumableDataProvider provider;
        private List<GameObject> rewardViews;
        
        [SerializeField] private GameObject container;
        [SerializeField] private Transform parentConsumable;
        [SerializeField] private Transform parentCurrency;

        [Header("RewardViews")] 
        [SerializeField] private RewardConsumableView consumablePrefab;
        [SerializeField] private RewardCurrencyView coinsPrefab;
        [SerializeField] private RewardCurrencyView expPrefab;

        [Header("AnimationComponent")] 
        [SerializeField] private Image background;
        [SerializeField] private CanvasGroup screen;

        public void OnDisable()
        {
            ClearAll();
        }

        public void Open()
        {
            container.SetActive(true);
            
            popupEffector.PlayPopupOpenAnimation(screen, background);
        }

        public void Close()
        {
            popupEffector.PlayPopupCloseAnimation(screen, background, () =>
            {
                ClearAll();
                container.SetActive(false);
                CoroutineManager.StartCoroutine(KillTwinsAndExitToMenu());
            });
        }

        public void ShowBattleReward(BattleData battleData)
        {
            provider ??= StaticDataProvider.Get<ConsumableDataProvider>();
            rewardViews ??= new List<GameObject>();

            battleData.Loot.ForEach(loot =>
            {
                switch (loot)
                {
                    case LootConsumable cons:
                        CreateConsumable(cons);
                        break;
                    
                    case LootCurrency curr:
                        CreateCoinsReward(curr);
                        break;
                }
            });
            
            if (battleData.Expirience > 0)
                CreateExpReward(battleData.Expirience);

            Open();
        }

        private void ClearAll()
        {
            if (rewardViews == null) return;
            
            rewardViews.ForEach(view => Destroy(view.gameObject));
            rewardViews.Clear();
        }

        private void CreateConsumable(LootConsumable consumable)
        {
            RewardConsumableView view = Instantiate(consumablePrefab, parentConsumable);
            ConsumableData data = provider.GetConsumable(consumable.consumableType);

            view.Icon = data.Sprite;
            view.NameConsumable = data.NameId;
            view.Description = data.DescriptionId;
            
            rewardViews.Add(view.gameObject);
        }
        
        private void CreateCoinsReward(LootCurrency currency)
        {
            RewardCurrencyView view = Instantiate(coinsPrefab, parentCurrency);

            view.Coins = currency.quantity.ToString();

            rewardViews.Add(view.gameObject);
        }
        
        private void CreateExpReward(int value)
        {
            RewardCurrencyView view = Instantiate(expPrefab, parentCurrency);

            view.Coins = value.ToString();

            rewardViews.Add(view.gameObject);
        }
        
        private IEnumerator KillTwinsAndExitToMenu()
        {
            DOTween.KillAll();
            yield return null;
            sceneLoader.Load(SceneEnum.Menu);
        }
    }
}