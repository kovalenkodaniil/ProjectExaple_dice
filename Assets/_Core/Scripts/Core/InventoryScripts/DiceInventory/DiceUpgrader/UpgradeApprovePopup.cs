using System;
using Core.Data;
using Managers;
using Popups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Core.InventoryScripts
{
    public class UpgradeApprovePopup : MonoBehaviour
    {
        [Inject] private Managers.Localization localization;
        [Inject] private PopupEffector _popupEffector;
        [Inject] private SoundManager _soundManager;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _background;
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _description;

        public event Action OnUpgradeApproved;
        public event Action OnClose;

        public void ShowApprove(DiceConfig diceConfig)
        {
            _container.SetActive(true);
            
            _popupEffector.PlayPopupOpenAnimation(_canvasGroup, _background);

            SetDescription(diceConfig);
        }

        public void Close()
        {
            _popupEffector.PlayPopupCloseAnimation(_canvasGroup, _background, () =>
            {
                _container.SetActive(false);
            
                OnClose?.Invoke();
            });
        }

        public void ApproveUpgrade()
        {
            _soundManager.PlayEffect(_soundManager.SoundList.BuySound);
            
            OnUpgradeApproved?.Invoke();
            
            Close();
        }

        private void SetDescription(DiceConfig diceConfig)
        {
            string description = localization.GetTranslate("loc_heromenu_upgrade_body");

            string[] tempArray = description.Split(' ');

            description = "";

            for (int i = 0; i < tempArray.Length; i++)
            {
                switch (tempArray[i])
                {
                    case "{diceName}":
                        tempArray[i] = $"\"{localization.GetTranslate(diceConfig.diceName)}\"";
                        break;
                    
                    case "{lvl}":
                        tempArray[i] = diceConfig.levelNumber;
                        break;
                    
                    case "{lvlUp}":
                        tempArray[i] = diceConfig.Upgrades[0].levelNumber;
                        break;
                    
                    case "{cost}":
                        tempArray[i] = $"{diceConfig.upgradeCost} <sprite name=\"CoinIcon\">";
                        break;
                }

                description += tempArray[i] + ' ';
            }

            _description.text = description;
        }
    }
}