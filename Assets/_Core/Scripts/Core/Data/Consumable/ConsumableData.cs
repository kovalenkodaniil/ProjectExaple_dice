using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils.Drawer.ListDisplay;

namespace Core.Data.Consumable
{
    public enum EnumConsumable
    {
        HealPotion = 1,
        ManaPotion = 2,
        LuckyTicket = 3,
        Palette = 4
    }
    
    [CreateAssetMenu(menuName = "Data/Items/Consumable")]
    public class ConsumableData : ScriptableObject
    {
        [SerializeField] private EnumConsumable consumableType;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite shadowSprite;
        [SerializeField, ListDisplay(ListDisplayAttribute.DisplayMode.Inline)] private List<Effect> effects;
  
        public string NameId => GetID();
        public string DescriptionId => GetDescriptionID();
        public string EffectId => GetEffectID();
        public EnumConsumable ConsumableType => consumableType;
        public List<Effect> Effects => effects;
        public Sprite ShadowSprite => shadowSprite;
        public Sprite IconInPanel => sprite;

        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }

        private string GetID()
        {
            //return $"consumable_name_{consumableType.ToString().ToLower()}";
            return consumableType switch
            {
                EnumConsumable.HealPotion => "Зелье Здоровья",
                EnumConsumable.ManaPotion => "Зелье Маны",
                EnumConsumable.LuckyTicket => "Счастливый билет",
                EnumConsumable.Palette => "Палитра",
                _ => ""
            };
        }

        private string GetDescriptionID()
        {
            //return $"consumable_description_{consumableType.ToString().ToLower()}";
            return consumableType switch
            {
                EnumConsumable.HealPotion => "Зелье восстанавливает здоровье.",
                EnumConsumable.ManaPotion => "Зелье восстанавливает ману.",
                EnumConsumable.LuckyTicket => "Выбранный кубик получает случайный цвет",
                EnumConsumable.Palette => "Снижает серость",
                _ => ""
            };
        }
        
        private string GetEffectID()
        {
            //return $"consumable_effect_{consumableType.ToString().ToLower()}";
            return consumableType switch
            {
                EnumConsumable.HealPotion => "здоровье",
                EnumConsumable.ManaPotion => "мана",
                EnumConsumable.LuckyTicket => "цвет",
                EnumConsumable.Palette => "серость",
                _ => ""
            };
        }
    }

    #region Editor

#if UNITY_EDITOR
    [CustomEditor(typeof(ConsumableData))]
    public class ConsumableDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ConsumableData instance = (ConsumableData)target;
            Rect rect = EditorGUILayout.GetControlRect(false, 120);
            instance.Sprite = (Sprite)EditorGUI.ObjectField(rect, "", instance.Sprite, typeof(Sprite), false);
            base.OnInspectorGUI();
        }
    }
#endif

    #endregion
}