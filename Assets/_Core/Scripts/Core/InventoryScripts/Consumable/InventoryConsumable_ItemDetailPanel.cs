using System;
using Core.Localization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class InventoryConsumable_ItemDetailPanel : MonoBehaviour
    {
        public event Action OnSelected;
        
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button btnSelect;
        [SerializeField] private Image iconConsumable;
        [SerializeField] private TMP_Text tmpName;
        [SerializeField] private TMP_Text tmpDescription;
        [SerializeField] private TMP_Text tmpEffectName;
        [SerializeField] private TMP_Text tmpEffectPower;
        [SerializeField] private GameObject content;

        public string Name { set => tmpName.text = value; }
        public string Description { set => tmpDescription.text = value; }
        public string EffectName { set => tmpEffectName.text = value; }
        public string EffectPower { set => tmpEffectPower.text = value; }
        public Sprite IconConsumable { set => iconConsumable.sprite = value; }
        
        public void SetFontSetting(FontSetting setting)
        {
            tmpName.font = setting.fontAsset;
            tmpName.fontMaterial = setting.fontMaterial;
            
            tmpDescription.font = setting.fontAsset;
            tmpDescription.fontMaterial = setting.fontMaterial;
        }

        public void SetActive(bool value)
        {
            content.SetActive(value);
        }

        public void PlayAppearanceContent()
        {
            canvasGroup.DOFade(1, 0.4f);
        }

        public void PlayHideContent(Action callback)
        {
            canvasGroup.DOFade(0, 0.4f).OnComplete(() => callback?.Invoke());
        }
        
        public void SetInteractableButton(bool isInteractable)
        {
            btnSelect.interactable = isInteractable;
        }

        public void OnClickSelected()
        {
            OnSelected?.Invoke();
        }
    }
}