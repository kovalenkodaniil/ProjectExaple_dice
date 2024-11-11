using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.InventoryScripts
{
    public class ClickCatcher : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClicked;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Кетчер");
            OnClicked?.Invoke();
        }
    }
}