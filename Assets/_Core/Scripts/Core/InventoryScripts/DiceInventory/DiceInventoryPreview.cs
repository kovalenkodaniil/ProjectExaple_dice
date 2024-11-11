using System;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class DiceInventoryPreview : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private DiceUIPreview _dice3D;
        
        [Header("Background")]
        [SerializeField] private Image _background;
        [SerializeField] private Sprite _defaultBackgroundSprite;
        [SerializeField] private Sprite _selectingBackgroundSprite;
        
        [Header("BackgroundDecor")]
        [SerializeField] private Image _backgroundDecor;
        
        [Header("Border")]
        [SerializeField] private Image _selectingBorder;
        [SerializeField] private Sprite _defaultBorderSprite;

        [Header("DiceUI")]
        [SerializeField] private Image _maxLevelPanel;
        [SerializeField] private Image _defaultLevelPanel;
        [SerializeField] private TMP_Text _diceQuantity;
        [SerializeField] private Image _diceCount;
        [SerializeField] private TMP_Text _diceLevel;
        
        [Header("Other")]
        [SerializeField] private Image _selectingEffect;
        [SerializeField] private Image _inBattleLabel;
        
        public event Action OnPreviewClicked;
        
        public void SetDiceCount(int quantity)
        {
            _diceCount.gameObject.SetActive(true);
            _diceQuantity.text = quantity.ToString();
        }

        public void SetDiceLevel(string level)
        {
            switch (level)
            {
                case "Максимальный":
                case "5":
                    _maxLevelPanel.gameObject.SetActive(true);
                    _defaultLevelPanel.gameObject.SetActive(false);
                    break;
                
                default:
                    _diceLevel.text = $"Level {level}";
                    
                    _maxLevelPanel.gameObject.SetActive(false);
                    _defaultLevelPanel.gameObject.SetActive(true);
                    break;
            }
        }

        public void SetDice(DiceConfig config)
        {
            _dice3D.gameObject.SetActive(true);
            
            _dice3D.InitializeInventory(config, 85);
        }
        
        public void SetInBattleVariant()
        {
            _inBattleLabel.gameObject.SetActive(true);
        }
        
        public void SetDefaultVariant()
        {
            _background.sprite = _defaultBackgroundSprite;
            _inBattleLabel.gameObject.SetActive(false);
        }
        
        public void PlayNewDiceAnimation()
        {
            _diceLevel.DOFade(1, 0.4f);
            _diceLevel.transform.DOScale(1, 0.4f);
        }

        public void PlayHideAnimation(Action callback = null)
        {
            _dice3D.PlayHideAnimation(callback);
        }
        
        public void PlayUpgradeAnimation(Action callback = null)
        {
            _diceLevel.DOFade(0, 0.4f);
            _diceLevel.transform.DOScale(0, 0.4f);

            _dice3D.PlayHideAnimation(callback);
        }
        
        public void SetSelectingVariant()
        {
            _background.sprite = _selectingBackgroundSprite;

            _selectingBorder.DOFade(1, 0.35f).SetDelay(0.05f);
            _selectingEffect.DOFade(1, 0.35f).SetDelay(0.05f);
            
            _selectingBorder.gameObject.SetActive(true);
            _selectingEffect.gameObject.SetActive(true);
            _inBattleLabel.gameObject.SetActive(false);
            
            _maxLevelPanel.gameObject.SetActive(false);
            _defaultLevelPanel.gameObject.SetActive(false);
        }
        
        public void HideSelectingEffect()
        {
            _selectingBorder.DOFade(0, 0.35f).SetDelay(0.05f).SetLink(gameObject);
            _selectingEffect.DOFade(0, 0.35f).SetDelay(0.05f).SetLink(gameObject);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            SetSelectingVariant();
            
            OnPreviewClicked?.Invoke();
        }
    }
}