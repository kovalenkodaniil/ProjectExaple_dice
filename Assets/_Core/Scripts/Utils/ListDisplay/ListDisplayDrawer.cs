#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Drawer.ListDisplay
{
    [CustomPropertyDrawer(typeof(ListDisplayAttribute))]

    public class ListDisplayDrawer : PropertyDrawer
    {
        private int propertyCount;
        private float nameWidth = 0.5f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Generic || !property.hasVisibleChildren)
            {
                base.OnGUI(position, property, label);
                return;
            }
            
            // Подсчитываем кол-во свойств
            propertyCount = 0;
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();
            SerializedProperty tempIterator = iterator.Copy();
            while (tempIterator.NextVisible(true) && !SerializedProperty.EqualContents(tempIterator, endProperty))
                propertyCount++;

            EditorGUI.BeginProperty(position, label, property);
            
            // PrefixLabel
            EditorGUI.PrefixLabel(position, new GUIContent($"{label.text.Split(" ")[1]}"));
            var prefLabSize = GUI.skin.label.CalcSize(new(string.Concat(Enumerable.Range(0, 4))));
            position = new(position.x + prefLabSize.x, position.y, position.width - prefLabSize.x, position.height);
            
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            DrawProperty(position, property, label);

            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty();
        }
        
        private void DrawProperty(Rect rect, SerializedProperty property, GUIContent label)
        {
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = iterator.GetEndProperty();
            
            float defaultWidth = (rect.width - (propertyCount - 1) * 2) / propertyCount;
            float nameOffset = defaultWidth * nameWidth;
            Rect fieldRect = new Rect(rect.x + nameOffset, rect.y, defaultWidth - nameOffset, EditorGUIUtility.singleLineHeight);
            
            iterator.NextVisible(true);
            while (!SerializedProperty.EqualContents(iterator, endProperty))
            {
                // Property Name
                var originalColor = GUI.contentColor;
                GUI.contentColor = Color.white * .7f;
                Rect lableRect = new Rect(fieldRect.x - nameOffset, fieldRect.y, nameOffset, fieldRect.height);
                EditorGUI.LabelField(lableRect, iterator.displayName);
                GUI.contentColor = originalColor;
                
                EditorGUI.PropertyField(fieldRect, iterator, GUIContent.none);
                fieldRect.x += defaultWidth + 2;
                iterator.NextVisible(false);
            }
        }
    }
}
#endif