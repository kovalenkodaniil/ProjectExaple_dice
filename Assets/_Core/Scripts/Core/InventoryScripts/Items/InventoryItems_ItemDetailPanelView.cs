using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_ItemDetailPanelView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _selectButton;
        [SerializeField] private CanvasGroup _content;

        public event Action OnItemSelected;

        public void PlayAppearanceContent() => _content.DOFade(1, 0.4f);
        
        public void PlayHideContent(Action callback)
        {
            _content.DOFade(0, 0.4f).OnComplete(() => callback?.Invoke());
        }

        public void SetName(string name) => _name.text = name;

        public void SetItemImage(Sprite sprite) => _icon.sprite = sprite;
        
        public void SetDescription(string description) => _description.text = description;

        public void SetActiveButton(bool isInteractable) => _selectButton.interactable = isInteractable;

        public void AddItemInBattle() => OnItemSelected?.Invoke();
    }
}