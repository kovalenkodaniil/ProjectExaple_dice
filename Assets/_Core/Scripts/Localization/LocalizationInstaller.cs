using System;
using System.Collections.Generic;
using System.Linq;
using Core.Localization;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizationInstaller : MonoBehaviour
    {
        [SerializeField] private List<LocalizationSetter> _texts;

        private Managers.Localization localization;
        private FontSettings _fontSettings;
        
        public void Initialize(Managers.Localization localization, FontSettings fontSettings)
        {
            this.localization = localization;
            _fontSettings = fontSettings;

            UpdateLocalization();

            this.localization.OnLanguageChange += UpdateLocalization;
        }

        public void OnDestroy()
        {
            if (localization != null) localization.OnLanguageChange -= UpdateLocalization;
        }

        public void ChangeFontSetting(TMP_Text text, TextTag tag)
        {
            FontSetting setting = _fontSettings.GetFontSetting(localization.Language, tag);

            if (text == null)
                Debug.Log($"text null {tag}");
            
            if (setting == null)
                Debug.Log($"setting {tag}");
            
            text.font = setting.fontAsset;
            text.fontMaterial = setting.fontMaterial;
        }

        private void UpdateLocalization()
        {
            _texts.ForEach(text => text.SetLocalization(localization, this));
        }

        [Button("FindText")]
        private void FindTextComponent()
        {
           _texts = GetComponentsInChildren<LocalizationSetter>(true).ToList();
        }
    }
}