using System;
using System.Collections.Generic;
using Core.Localization;
using Localization;
using TMPro;
using UnityEngine;

namespace Popups.PopupReward
{
    public class RewardView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _views;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _count;
        [SerializeField] private TMP_Text _name;

        public event Action OnRewardButtonClicked;

        public void SetName(string name, FontSettings fontSettings, string language)
        {
            FontSetting headerSetting = fontSettings.GetFontSetting(language, TextTag.ExoBold);

            _name.font = headerSetting.fontAsset;
            _name.fontMaterial = headerSetting.fontMaterial;

            _name.text = name;
        }

        public void SetDescription(string description, FontSettings fontSettings, string language)
        {
            FontSetting nameSetting = fontSettings.GetFontSetting(language, TextTag.ExoBold);
            
            _description.font = nameSetting.fontAsset;
            _description.fontMaterial = nameSetting.fontMaterial;

            _description.text = description;
        }

        public void SetCount(string count) => _count.text = $"+{count}";

        public void Show(bool isEnable)
        {
            _views.ForEach(view=>view.gameObject.SetActive(isEnable));
        }

        public virtual void PlayHideAnimation() { }

        public void GetReward() => OnRewardButtonClicked?.Invoke();
    }
}