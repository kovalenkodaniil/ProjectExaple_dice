using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class InventorySkills_SkillDetailPanelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _spellName;
        [SerializeField] private Image _spellImage;
        [SerializeField] private TMP_Text _spellDescription;
        [SerializeField] private TMP_Text _spellManaCost;
        [SerializeField] private TMP_Text _spellCooldown;
        [SerializeField] private Button _selectButton;
        [SerializeField] private CanvasGroup _spellContent;

        public event Action OnSpellSlected;

        public void PlayAppearanceContent() => _spellContent.DOFade(1, 0.4f);
        
        public void PlayHideContent(Action callback)
        {
            _spellContent.DOFade(0, 0.4f).OnComplete(() => callback?.Invoke());
        }

        public void SetName(string name) => _spellName.text = name;

        public void SetSpellImage(Sprite sprite) => _spellImage.sprite = sprite;
        
        public void SetDescription(string description) => _spellDescription.text = description;
        
        public void SetManaCost(int manaCost) => _spellManaCost.text = manaCost.ToString();
        
        public void SetCooldown(int cooldown) => _spellCooldown.text = cooldown.ToString();

        public void SetActiveButton(bool isInteractable) => _selectButton.interactable = isInteractable;

        public void AddSpellInBattle() => OnSpellSlected?.Invoke();
    }
}