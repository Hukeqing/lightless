using UnityEngine;
using UnityEditor;

namespace GameManager.Editor
{
    [CustomPropertyDrawer(typeof(EnemyData))]
    public class EnemyDataEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var tmp = GameDataManager.GetColor((Rarity) property.FindPropertyRelative("enemyRarity").enumValueIndex);
            
            var rectPosition = position;

            position = EditorGUI.PrefixLabel(position, label);
            var index = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            rectPosition.width -= position.width;
            rectPosition.y += rectPosition.height / 2;
            rectPosition.height /= 2;

            EditorGUI.DrawRect(rectPosition, tmp);

            position.height /= 2;
            var enemyName = new Rect(position.x, position.y, 130, position.height);
            var enemyRarity = new Rect(position.x + 135, position.y, position.width - 135, position.height);

            var enemyPrefab = new Rect(position.x, position.y + position.height, position.width, position.height);

            //绘制属性
            EditorGUI.PropertyField(enemyName, property.FindPropertyRelative("enemyName"), GUIContent.none);
            EditorGUI.PropertyField(enemyRarity, property.FindPropertyRelative("enemyRarity"), GUIContent.none);
            EditorGUI.PropertyField(enemyPrefab, property.FindPropertyRelative("enemyPrefab"), GUIContent.none);

            //重新设置为原来的层级
            EditorGUI.indentLevel = index;

            EditorGUI.EndProperty();
        }
    }
}