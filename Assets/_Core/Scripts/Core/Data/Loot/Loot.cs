using System;
using Core.Data.Consumable;
using PlayerScripts;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Data
{
    public enum EnumLoot
    {
        Currency = 0,
        Consumable = 1,
    }
    
    [Serializable]
    public abstract class Loot
    {
        public int quantity;
        public abstract EnumLoot LootType { get; }
    }

    [Serializable]
    public class LootConsumable : Loot
    {
        public EnumConsumable consumableType;
        public override EnumLoot LootType => EnumLoot.Consumable;
    }

    [Serializable]
    public class LootCurrency : Loot
    {
        public EnumCurrency currencyType;
        public override EnumLoot LootType => EnumLoot.Currency;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Loot), true)]
    public class LootDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            Rect popupRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Loot loot = property.managedReferenceValue as Loot;

            EnumLoot selectedLootType = loot?.LootType ?? EnumLoot.Consumable;
            EnumLoot newLootType = (EnumLoot)EditorGUI.EnumPopup(popupRect, "Loot Type", selectedLootType);

            if (newLootType != selectedLootType || loot == null)
            {
                switch (newLootType)
                {
                    case EnumLoot.Consumable:
                        property.managedReferenceValue = new LootConsumable();
                        break;
                    case EnumLoot.Currency:
                        property.managedReferenceValue = new LootCurrency();
                        break;
                }

                property.serializedObject.ApplyModifiedProperties();
                loot = property.managedReferenceValue as Loot;
            }
            
            if (loot is LootConsumable consumableLoot)
            {
                position = DrawConsumable(position, property);
                if (loot.quantity == 0)
                    loot.quantity = 1;
            }
            else if (loot is LootCurrency moneyLoot)
            {
                position = DrawCurrency(position, property);
                if (loot.quantity == 0)
                    loot.quantity = 1;
            }

            EditorGUI.EndProperty();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Loot loot = property.managedReferenceValue as Loot;

            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (loot is LootConsumable or LootCurrency)
            {
                height *= 2;
            }

            return height;
        }

        private Rect DrawConsumable(Rect position, SerializedProperty property)
        {
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            float consumableWidth = position.width * 0.6f - 5;

            Rect consumableRect = new Rect(position.x, position.y, consumableWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(consumableRect, property.FindPropertyRelative("consumableType"), new GUIContent("Consumable Type"));
                
            EditorGUI.LabelField(new(position.x + consumableWidth + 5, position.y, 60, EditorGUIUtility.singleLineHeight), "Quantity");
            EditorGUI.PropertyField(new(position.x + consumableWidth + 5 + 60, position.y, 60, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("quantity"), GUIContent.none);
            
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            return position;
        }
        
        private Rect DrawCurrency(Rect position, SerializedProperty property)
        {
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            float consumableWidth = position.width * 0.6f - 5;

            Rect consumableRect = new Rect(position.x, position.y, consumableWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(consumableRect, property.FindPropertyRelative("currencyType"), new GUIContent("Currency Type"));
                
            EditorGUI.LabelField(new(position.x + consumableWidth + 5, position.y, 60, EditorGUIUtility.singleLineHeight), "Quantity");
            EditorGUI.PropertyField(new(position.x + consumableWidth + 5 + 60, position.y, 60, EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("quantity"), GUIContent.none);
            
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            return position;
        }
    }
#endif
}