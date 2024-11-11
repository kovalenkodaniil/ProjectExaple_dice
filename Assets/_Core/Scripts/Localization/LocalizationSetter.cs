using Managers;
using TMPro;
using UnityEngine;

namespace Localization
{
    public enum TextTag
    {
        None = 0,
        Regular,
        ButtonText,
        CoinsShadow,
        Counter,
        Header,
        MainMenuSelected,
        MainMenuShadow,
        PopupReward,
        PopupRewardShadow,
        PopupLose,
        SelectorButton,
        SelectorButtonOff,
        XCounter,
        ExoBold,
        PopupLoseBack,
        RewardHeader,
        DiceUpgrade
    }

    public class LocalizationSetter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TextTag _textType;
        [SerializeField] private string _localizationTag;

        private Managers.Localization localization;
        private LocalizationInstaller _localizationInstaller;

        public void SetLocalization(Managers.Localization localization, LocalizationInstaller localizationInstaller)
        {
            this.localization = localization;
            _localizationInstaller = localizationInstaller;
            
            _text.text = localization.GetTranslate(_localizationTag);
            
            localizationInstaller.ChangeFontSetting(_text, _textType);
        }

        public void UpdateLocalization()
        {
            _text.text = localization.GetTranslate(_localizationTag);
            _localizationInstaller.ChangeFontSetting(_text, _textType);
        }

        public void SetTag(string localizationTag)
        {
            _localizationTag = localizationTag;
        }
    }
}