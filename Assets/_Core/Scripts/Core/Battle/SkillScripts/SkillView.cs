using System;
using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Battle.SkillScripts
{
    public class SkillView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TooltipWindow _tooltip;
        [SerializeField] private TMP_Text _cooldown;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Image _manaCostCounter;
        [SerializeField] private Image _cooldownMask;
        [SerializeField] private Image _icon;
        [SerializeField] private Transform _effectParent;

        public Sprite SkillIcon { set => _icon.sprite = value; }
        public Transform EffectParent => _effectParent;

        public event Action OnCliked;
        
        public bool IsInteractable;
        public bool IsAvailable;

        public void SetTooltip(string value)
        {
            _tooltip.SetText(value);
        }

        public void SetManaCost(string value)
        {
            _manaCostCounter.gameObject.SetActive(value != "0");

            _cost.text = value;
        }

        public void SetDisableState(bool isDisabled)
        {
            _cooldownMask.gameObject.SetActive(isDisabled);
            _manaCostCounter.gameObject.SetActive(isDisabled);
        }

        public void EnableCooldown(bool isEnabling, string value = "0")
        {
            _cooldown.gameObject.SetActive(isEnabling);
            _cooldownMask.gameObject.SetActive(isEnabling);
            _manaCostCounter.gameObject.SetActive(!isEnabling);

            _cooldown.text = value;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsInteractable) return;
            
            OnCliked?.Invoke();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltip.gameObject.SetActive(true);

            transform.DOScale(0.1f, 0.1f).SetRelative().SetLink(gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltip.gameObject.SetActive(false);
            
            transform.DOScale(1, 0.1f).SetLink(gameObject);
        }
    }
}