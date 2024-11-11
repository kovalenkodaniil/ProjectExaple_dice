using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using Core.Localization;
using DG.Tweening;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class InventoryPreview : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private DiceUIPreview _dice3D;
        [SerializeField] private DiceUIPreview _diceDragView;
        [SerializeField] private Image _spellDragIcon;
        
        [Header("Background")]
        [SerializeField] private Image _background;
        [SerializeField] private Sprite _selectingBackgroundSprite;
        [SerializeField] private Sprite _lockBackgroundSprite;
        
        [Header("Icon")]
        [SerializeField] private Image _icon;
        [SerializeField] private Color _defaultIconColor;
        [SerializeField] private Color _selectingIconColor;
        
        [Header("DiceUI")]
        [SerializeField] private Image _maxLevelPanel;
        [SerializeField] private Image _defaultLevelPanel;
        [SerializeField] private TMP_Text _diceQuantity;
        [SerializeField] private Image _diceCount;
        [SerializeField] private TMP_Text _diceLevel;
        
        [Header("Other")]
        [SerializeField] private Image _selectingEffect;
        [SerializeField] private Image _inBattleLabel;
        [SerializeField] private Color _lockColor;
        
        [Header("Localization")]
        [SerializeField] private LocalizationInstaller _localizationInstaller;

        public event Action OnSpellPreviewClicked;
        public event Action OnPreviewDragStarted;

        private bool _isInteractable;
        private bool _isDragable;

        public Image Icon => _icon;
        
        private void Start()
        {
            _selectingEffect.gameObject.SetActive(false);
        }

        public void SetLocalization(Managers.Localization localization, FontSettings _fontSettings)
        {
            _localizationInstaller.Initialize(localization, _fontSettings);
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

        public void PlayNewDiceAnimation()
        {
            _diceLevel.DOFade(1, 0.4f);
            _diceLevel.transform.DOScale(1, 0.4f);
        }

        public void SetDragable(bool isDragable) => _isDragable = isDragable;

        public void SetIcon(Sprite spellIcon)
        {
            _icon.sprite = spellIcon;
            _spellDragIcon.sprite = spellIcon;
        }

        public void SetDiceCount(int quantity)
        {
            _diceCount.gameObject.SetActive(true);
            _diceQuantity.text = quantity.ToString();
        }

        public void SetDiceLevel(string level, Managers.Localization localization, FontSetting fontSetting)
        {
            _diceLevel.text = localization.GetTranslate("loc_heromenu_tip_dice_level");

            switch (level)
            {
                case "Максимальный":
                case "3":
                    _maxLevelPanel.gameObject.SetActive(true);
                    _defaultLevelPanel.gameObject.SetActive(false);
                    break;
                
                case "1":
                    _diceLevel.text += " I";
                    
                    _maxLevelPanel.gameObject.SetActive(false);
                    _defaultLevelPanel.gameObject.SetActive(true);
                    break;
                
                case "2":
                    _diceLevel.text += " II";
            
                    _maxLevelPanel.gameObject.SetActive(false);
                    _defaultLevelPanel.gameObject.SetActive(true);
                    break;
            }
        }

        public void SetDice(DiceConfig config)
        {
            _dice3D.gameObject.SetActive(true);
            _icon.gameObject.SetActive(false);
            
            _dice3D.InitializeInventory(config, 85);
            _diceDragView.InitializeInventory(config, 85);
        }

        public void SetInBattleVariant()
        {
            _isInteractable = true;
            _inBattleLabel.gameObject.SetActive(true);
        }
        
        public void SetDefaultVariant(bool isDice = false)
        {
            _isInteractable = true;
            
            _inBattleLabel.gameObject.SetActive(false);

            if (isDice)
            {
                _icon.color = Color.white;
            }
            else
            {
                _icon.color = _defaultIconColor;
            }
        }

        public void HideSelectingEffect()
        {
            _selectingEffect.DOFade(0, 0.35f).SetDelay(0.05f).SetLink(gameObject);
        }

        public void SetSelectingVariant()
        {
            _isInteractable = true;

            //_background.sprite = _selectingBackgroundSprite;
            
            _icon.color = _selectingIconColor;
            
            _selectingEffect.DOFade(1, 0.35f).SetDelay(0.05f);
            
            _selectingEffect.gameObject.SetActive(true);
            _inBattleLabel.gameObject.SetActive(false);
        }
        
        public void SetBlockingVariant()
        {
            _isInteractable = true;

            _background.sprite = _lockBackgroundSprite;
            
            _icon.color = _lockColor;

            _selectingEffect.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isInteractable) return;
            
            SetSelectingVariant();
            
            OnSpellPreviewClicked?.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isDragable) return;
            
            if (_diceDragView != null)
            {
                _diceDragView.transform.localPosition = new Vector3(0, 0, -1.2f);
                _diceDragView.gameObject.SetActive(true);
                
                OnPreviewDragStarted?.Invoke();
            }

            if (_spellDragIcon)
            {
                _spellDragIcon.transform.localPosition = new Vector3(0, 0, -1.2f);
                _spellDragIcon.gameObject.SetActive(true);
                _spellDragIcon.color = _icon.color;
                
                OnPreviewDragStarted?.Invoke();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragable) return;
            
            if (_diceDragView != null)
            {
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
                
                _diceDragView.gameObject.SetActive(false);

                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent<DiceInBattlePreview>(out DiceInBattlePreview battlePreview))
                    {
                        battlePreview.SelectPreview();
                        return;
                    }
                }
            }
            
            if (_spellDragIcon)
            {
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
                
                _spellDragIcon.gameObject.SetActive(false);

                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent<InBattlePreview>(out InBattlePreview battlePreview))
                    {
                        battlePreview.SelectPreview();
                        return;
                    }
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragable) return;
            
            if (_diceDragView != null)
            {
                _diceDragView.transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
            }
            
            if (_spellDragIcon != null)
            {
                _spellDragIcon.transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
            }
        }
    }
}