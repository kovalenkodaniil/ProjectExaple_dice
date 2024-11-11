using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Popups
{
    public class EdgeColorView : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClicked;
        
        [SerializeField] private Image _icon;

        public Color IconColor { set => _icon.color = value; }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}