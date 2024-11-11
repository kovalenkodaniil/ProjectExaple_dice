using Core.Features.Talents.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Features.Talents.Scripts
{
    public class Talent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TalentData data;
        [SerializeField] private GameObject description;
        [SerializeField] private Button btn;
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBg;
        [SerializeField] private TMP_Text lbDescription;
        [SerializeField] private TMP_Text lvLevel;
        
        public string LevelText { set => lvLevel.text = value; }
        public string DescriptionText { set => lbDescription.text = value; }
        public TalentData Data => data;

        private void Awake()
        {
            imgIcon.sprite = data.sprIcon;
            SetVisibleDescription(false);
        }

        public void SetBackgroud(Sprite sprite)
        {
            imgBg.sprite = sprite;
        }

        public void SetVisibleDescription(bool value)
        {
            description.gameObject.SetActive(value);
        }

        public void AddListener(UnityAction list)
        {
            btn.onClick.AddListener(list);
        }

        public void RemoveListener(UnityAction list)
        {
            btn.onClick.RemoveListener(list);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetVisibleDescription(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetVisibleDescription(false);
        }
    }
}