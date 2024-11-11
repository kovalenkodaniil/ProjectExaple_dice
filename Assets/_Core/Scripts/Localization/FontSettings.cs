using System;
using System.Collections.Generic;
using Localization;
using TMPro;
using UnityEngine;

namespace Core.Localization
{
    [CreateAssetMenu(fileName = "FontSettings", menuName = "Setting/Create new FontSettings")]
    public class FontSettings : ScriptableObject
    {
        public List<LanguageFonts> fontsForLanguage;

        public FontSetting GetFontSetting(string language, TextTag tag)
        {
            LanguageFonts languageFonts = fontsForLanguage.Find(fontLanguage => fontLanguage.language == language);
            return languageFonts.fonts.Find(fontSetting => fontSetting.tag == tag);
        }
    }

    [Serializable]
    public class FontSetting
    {
        public TextTag tag;
        public TMP_FontAsset fontAsset;
        public Material fontMaterial;
    }
    
    [Serializable]
    public class LanguageFonts
    {
        public string language;
        public List<FontSetting> fonts;
    }
}