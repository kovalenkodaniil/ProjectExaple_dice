using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Data;
using Core.Data;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace Managers
{
    public class Localization
    {
        private const string PLAYER_NAME_KEY = "{Name}";

        [Inject] private GameDataConfig _gameDataConfig;
        [Inject] private Player _player;

        public string Language { get; private set; }

        private Dictionary<string, Dictionary<string, string>> _localizationDictionary;

        public event Action OnLanguageChange;

        public string GetTranslate(string key)
        {
            return _localizationDictionary.ContainsKey(key) ? _localizationDictionary[Language][key] : key;
        }

        public static string Translate(string key) => key;

        public void GetRepliсaTranslate(ref string replica)
        {
            if (!_localizationDictionary[Language].ContainsKey(replica)) return;

            replica = _localizationDictionary[Language][replica];

            if (replica.Contains(PLAYER_NAME_KEY))
                replica = replica.Replace(PLAYER_NAME_KEY, _player.name);
        }

        public void GetEventTranslate(ref string eventText)
        {
            if (_localizationDictionary[Language].ContainsKey(eventText))
                eventText = _localizationDictionary[Language][eventText];
        }

        public void SetLanguage(string language)
        {
            if (Language == language) return;

            Language = language;
            OnLanguageChange?.Invoke();
        }

        public void UpdateLocalization()
        {
            _localizationDictionary = new Dictionary<string, Dictionary<string, string>>();

            _gameDataConfig.LocalizationData.localizations.ForEach(localization =>
            {
                LocalizationScheme localizationScheme = JsonUtility.FromJson<LocalizationScheme>(localization.text);

                SetLocalization(localizationScheme);
            });
        }
        
        public static string GetActionTranslate(string key)
        {
            return key.ToLower() switch
            {
                nameof(LocalizationKeys.attack)          => "Атака",
                nameof(LocalizationKeys.heal)            => "Лечение",
                nameof(LocalizationKeys.mana)            => "Мана",
                nameof(LocalizationKeys.empty)           => "Пусто",
                nameof(LocalizationKeys.armor)           => "Броня",
                nameof(LocalizationKeys.nolethaldamage)  => "Не смертельный урон",
                nameof(LocalizationKeys.skill)           => "Умение",
                nameof(LocalizationKeys.fixation)        => "Закреп",
                nameof(LocalizationKeys.armorBuff)       => "Увеличение брони",
                nameof(LocalizationKeys.damageBuff)      => "Увеличение повреждений",
                nameof(LocalizationKeys.spike)           => "Шип",
                nameof(LocalizationKeys.magicarmor)      => "Магическая броня",
                nameof(LocalizationKeys.stun)            => "Оглушение",
                nameof(LocalizationKeys.magicarrow)      => "Магическая стрела",
                nameof(LocalizationKeys.mastery)         => "Мастерство",
                nameof(LocalizationKeys.connection)      => "Соединение",
                nameof(LocalizationKeys.invulnerability) => "Неуязвимость",
                nameof(LocalizationKeys.trap)            => "Ловушка",
                _ => ""
            };
        } 
        
        public static string GetSpellTranslate(string key)
        {
            return key.ToLower() switch
            {
                nameof(LocalizationKeys.loc_heromenu_button_spell5) => "Барьер",
                nameof(LocalizationKeys.loc_heromenu_button_spell4) => "Бастион",
                nameof(LocalizationKeys.loc_heromenu_button_spell1) => "Огненный шар",
                nameof(LocalizationKeys.loc_heromenu_button_spell3) => "Ярость",
                nameof(LocalizationKeys.loc_heromenu_button_spell6) => "Магический взрыв",
                nameof(LocalizationKeys.loc_heromenu_button_spell7) => "Мастерство",
                nameof(LocalizationKeys.loc_heromenu_button_spell2) => "Оглушение",
                nameof(LocalizationKeys.loc_heromenu_button_spell8) => "Соединение",
                nameof(LocalizationKeys.loc_heromenu_button_skill1) => "Соединение",
                nameof(LocalizationKeys.loc_heromenu_button_skill2) => "Соединение",
                nameof(LocalizationKeys.loc_heromenu_button_skill3) => "Соединение",
                _ => ""
            };
        } 

        private void SetLocalization(LocalizationScheme scheme)
        {
            string language = scheme.localization.Find(x => x.key == "language").text;

            if (string.IsNullOrEmpty(language))
                return;

            if (_localizationDictionary.ContainsKey(language))
                _localizationDictionary.Remove(language);

            _localizationDictionary.Add(language, new());

            foreach (var item in scheme.localization)
            {
                if (_localizationDictionary[language].ContainsKey(item.key))
                    Debug.Log(item.key);

                _localizationDictionary[language].Add(item.key, item.text);
            }
        }
    }
    
    

    [Serializable]
    public class LocalizationScheme
    {
        public List<LocalizationItem> localization;
    }

    [Serializable]
    public class LocalizationItem
    {
        public string key;
        public string text;
    }
}

public static class LocalizationKeys
{
    // Menu
    public const string menu_play     = "ИГРАТЬ";
    public const string menu_talents  = "ТАЛАНТЫ";
    
    // Talents
    public const string talents_header = "Таланты";
    
    // Common
    public const string com_exp = "Опыт";
    public const string com_health = "Здоровье";
    public const string com_fixation = "Закрепы";
    public const string com_mana = "Мана";
    public const string com_magic_arrow = "Магическая стрела";
    public const string com_magic_shield = "Магический щит";
    
    // Effects
    public const string attack          = "Атака";
    public const string heal            = "Лечение";
    public const string mana            = "Мана";
    public const string empty           = "Пусто";
    public const string armor           = "Броня";
    public const string nolethaldamage  = "Не смертельный урон";
    public const string skill           = "Умение";
    public const string fixation        = "Закреп";
    public const string armorBuff       = "Увеличение брони";
    public const string damageBuff      = "Увеличение повреждений";
    public const string spike           = "Шип";
    public const string magicarmor      = "Магическая броня";
    public const string stun            = "Оглушение";
    public const string magicarrow      = "Магическая стрела";
    public const string mastery         = "Мастерство";
    public const string connection      = "Соединение";
    public const string invulnerability = "Неуязвимость";
    public const string trap            = "Ловушка";
    
    // Enemies
    public const string enemy_inquisitor     = "Инквизитор";
    public const string enemy_highinquisitor = "Старший Инквизитор";
    public const string enemy_guardsman      = "Гвардеец";
    
    // Consumables
    public const string consumable_healpotion = "Зелье Здоровья";
    public const string consumable_manapotion = "Зелье Маны";
    
    // Заклинания
    public const string loc_heromenu_button_spell5 = "Барьер";
    public const string loc_heromenu_button_spell4 = "Бастион";
    public const string loc_heromenu_button_spell1 = "Огненный шар";
    public const string loc_heromenu_button_spell3 = "Ярость";
    public const string loc_heromenu_button_spell6 = "Магический взрыв";
    public const string loc_heromenu_button_spell7 = "Мастерство";
    public const string loc_heromenu_button_spell2 = "Оглушение";
    public const string loc_heromenu_button_spell8 = "Соединение";
    public const string loc_heromenu_button_skill1 = "Переброс";
    public const string loc_heromenu_button_skill2 = "Выбрать цвет";
    public const string loc_heromenu_button_skill3 = "Рандомный цвет";

    
    // PopupLoading
    public const string popupLoading_loading = "Загрузка...";
    
    // PopupPrepareBattle
    public const string popupBattlePrepare_fight = "В БОЙ!";
    public const string popupBattlePrepare_preparationForBattle = "Подготовка к бою";
    public const string popupBattlePrepare_change = "ИЗМЕНИТЬ";
    public const string popupBattlePrepare_yourSpells = "Ваши заклинания";
    public const string popupBattlePrepare_yourItems = "Ваши предметы";
    public const string popupBattlePrepare_yourCubes = "Ваши кубы";
    
    // PopupInventory
    public const string popupInventory_btnDices = "Кубы";
    public const string popupInventory_btnSpells = "Заклинания";
    public const string popupInventory_btnConsumables = "Расходники";
    public const string popupInventory_btnItems = "Предметы";
}