using Managers;
using TMPro;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class InventoryTabButton : MonoBehaviour
    {
        [SerializeField] private InventoryPopup.EIntentoryTab type;
        [SerializeField] private TMP_Text lbName;
        [SerializeField] private GameObject gSelect;

        public InventoryPopup.EIntentoryTab Type => type;

        public string Text { set => lbName.text = value; }

        public void SetEnable(bool value)
        {
            gSelect.SetActive(value);
        }
    }
}