using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Popups.PopupReward
{
    public class SpellRewardView : RewardView
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Color _spellColor;

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
            _icon.color = _spellColor;
        }
    }
}