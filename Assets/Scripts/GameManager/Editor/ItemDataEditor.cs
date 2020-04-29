using UnityEngine;
using UnityEditor;

namespace GameManager.Editor
{
    [CustomPropertyDrawer(typeof(ItemData))]
    public class ItemDataEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 3;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var tmp = Color.white;
            switch (property.FindPropertyRelative("itemRarity").enumValueIndex)
            {
                case 1:
                    tmp = Color.green;
                    break;
                case 2:
                    tmp = Color.blue;
                    break;
                case 3:
                    tmp = Color.magenta;
                    break;
                case 4:
                    tmp = Color.yellow;
                    break;
                case 5:
                    tmp = Color.red;
                    break;
            }

            position = EditorGUI.PrefixLabel(position, label);
            var index = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.DrawRect(position, tmp);
            
            position.height /= 3;
            var itemName = new Rect(position.x, position.y, 130, position.height);
            var itemClass = new Rect(position.x + 135, position.y, 50, position.height);
            var itemRarity = new Rect(position.x + 190, position.y, position.width - 190, position.height);

            var describe = new Rect(position.x, position.y + position.height, position.width, position.height);

            var itemPrefab = new Rect(position.x, position.y + 2 * position.height, position.width, position.height);

            //绘制属性
            EditorGUI.PropertyField(itemClass, property.FindPropertyRelative("itemClass"), GUIContent.none);
            EditorGUI.PropertyField(itemRarity, property.FindPropertyRelative("itemRarity"), GUIContent.none);
            EditorGUI.PropertyField(itemName, property.FindPropertyRelative("itemName"), GUIContent.none);
            EditorGUI.PropertyField(describe, property.FindPropertyRelative("describe"), GUIContent.none);
            EditorGUI.PropertyField(itemPrefab, property.FindPropertyRelative("itemPrefab"), GUIContent.none);

            //重新设置为原来的层级
            EditorGUI.indentLevel = index;

            EditorGUI.EndProperty();
        }
    }
}