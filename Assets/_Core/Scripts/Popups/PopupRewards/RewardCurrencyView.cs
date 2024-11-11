using TMPro;
using UnityEngine;

namespace Popups.PopupReward
{
    public class RewardCurrencyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpCounter;

        public string Coins { set => tmpCounter.text = $"+ {value}"; }
    }
}