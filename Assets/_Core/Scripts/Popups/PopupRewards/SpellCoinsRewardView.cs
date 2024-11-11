using Core.Localization;
using Localization;
using TMPro;
using UnityEngine;

namespace Popups.PopupReward
{
    public class SpellCoinsRewardView : SpellRewardView
    {
        [SerializeField] private TMP_Text _spellName;
        
        public void SetSmallName(string name, FontSettings fontSettings, string language)
        {
            FontSetting nameSetting = fontSettings.GetFontSetting(language, TextTag.Regular);
            
            _spellName.font = nameSetting.fontAsset;
            _spellName.fontMaterial = nameSetting.fontMaterial;

            _spellName.text = name;
        }
    }
}