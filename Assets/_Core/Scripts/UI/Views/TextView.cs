using TMPro;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class TextView : MonoBehaviour
    {        
        [SerializeField] private TMP_Text lb;

        public string Text { set => lb.text = value; }
        
        public TMP_Text Lb => lb;
    }
}