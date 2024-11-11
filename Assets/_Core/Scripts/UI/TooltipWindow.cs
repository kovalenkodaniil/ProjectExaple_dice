using Core.Localization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TooltipWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string text) => _text.text = text;
        public void SetFont(FontSetting setting)
        {
            _text.font = setting.fontAsset;
            _text.material = setting.fontMaterial;
        }
    }
}