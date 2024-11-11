using Core.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class UpgradeEdgePreview : EdgePreview
    {
        [SerializeField] private Image _border;
        [SerializeField] private Sprite _defaultBorder;
        [SerializeField] private Sprite _selectingBorder;
        [SerializeField] private Color _defaultIconColor;
        [SerializeField] private Color _selectedIconColor;

        public void EnableSelectedMode(bool isEnable)
        {
            _edgeIcon.color = isEnable ? _selectedIconColor : _defaultIconColor;

            _border.sprite = isEnable ? _selectingBorder : _defaultBorder;
        }
    }
}