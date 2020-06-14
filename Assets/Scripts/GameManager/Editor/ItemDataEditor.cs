using UnityEngine;
using UnityEditor;

namespace GameManager.Editor
{
    [CustomPropertyDrawer(typeof(ItemData))]
    public class ItemDataEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var tmp = GameDataManager.GetColor((Rarity) property.FindPropertyRelative("dataRarity").enumValueIndex);

            var rectPosition = position;

            position = EditorGUI.PrefixLabel(position, label);
            var index = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            rectPosition.width -= position.width;
            rectPosition.y += rectPosition.height / 2;
            rectPosition.height /= 2;

            EditorGUI.DrawRect(rectPosition, tmp);

            position.height /= 2;
            var itemName = new Rect(position.x, position.y, 100, position.height);
            var itemClass = new Rect(position.x + 105, position.y, 50, position.height);
            var itemRarity = new Rect(position.x + 160, position.y, position.width - 160, position.height);

            if ((ItemClass) property.FindPropertyRelative("itemClass").enumValueIndex == 0)
            {
                var itemPrefab = new Rect(position.x, position.y + position.height, position.width / 2 - 5,
                    position.height);
                var itemSprite = new Rect(position.x + position.width / 2 + 5, position.y + position.height,
                    position.width / 2 - 5, position.height);

                EditorGUI.PropertyField(itemPrefab, property.FindPropertyRelative("dataPrefab"), GUIContent.none);
                EditorGUI.PropertyField(itemSprite, property.FindPropertyRelative("itemSprite"), GUIContent.none);
            }
            else
            {
                var itemPrefab = new Rect(position.x, position.y + position.height, position.width,
                    position.height);
                EditorGUI.PropertyField(itemPrefab, property.FindPropertyRelative("dataPrefab"), GUIContent.none);
            }

            //绘制属性
            EditorGUI.PropertyField(itemClass, property.FindPropertyRelative("itemClass"), GUIContent.none);
            EditorGUI.PropertyField(itemRarity, property.FindPropertyRelative("dataRarity"), GUIContent.none);
            EditorGUI.PropertyField(itemName, property.FindPropertyRelative("dataName"), GUIContent.none);

            //重新设置为原来的层级
            EditorGUI.indentLevel = index;

            EditorGUI.EndProperty();
        }
    }
}