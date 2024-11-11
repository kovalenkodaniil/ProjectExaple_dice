using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Battle.Consumables
{
    public class ConsumableBattleWidget : MonoBehaviour
    {
        [SerializeField] private Image img;
        [SerializeField] private Button btn;
        
        public Sprite Sprite { set => img.sprite = value; }

        public void AddListener(UnityAction action) => btn.onClick.AddListener(action);
        
        public void RemoveListener(UnityAction action) => btn.onClick.RemoveListener(action);
    }
}