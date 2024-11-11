using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popups.PopupReward
{
    public class RewardConsumableView : MonoBehaviour
    {
        [SerializeField] private Image iconConsumable;
        [SerializeField] private TMP_Text tmpName;
        [SerializeField] private TMP_Text tmpDescription;

        public Sprite Icon { set => iconConsumable.sprite = value; }
        public string NameConsumable { set => tmpName.text = value; }
        public string Description { set => tmpDescription.text = value; }
    }
}