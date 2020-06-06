using UnityEditor;
using UnityEngine;

namespace GameManager.Editor
{
    [CustomPropertyDrawer(typeof(RoomData))]
    public class RoomDataEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 3;
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
            rectPosition.y += rectPosition.height / 3;
            rectPosition.height = rectPosition.height / 3 * 2;

            EditorGUI.DrawRect(rectPosition, tmp);

            position.height /= 3;
            var roomName = new Rect(position.x, position.y, 130, position.height);
            var roomRarity = new Rect(position.x + 135, position.y, position.width - 135, position.height);

            var roomPrefab = new Rect(position.x, position.y + position.height, position.width, position.height);
            var roomDescribe = new Rect(position.x, position.y + position.height * 2, position.width, position.height);

            //绘制属性
            EditorGUI.PropertyField(roomName, property.FindPropertyRelative("dataName"), GUIContent.none);
            EditorGUI.PropertyField(roomRarity, property.FindPropertyRelative("dataRarity"), GUIContent.none);
            EditorGUI.PropertyField(roomPrefab, property.FindPropertyRelative("dataPrefab"), GUIContent.none);
            EditorGUI.PropertyField(roomDescribe, property.FindPropertyRelative("roomDescribe"), GUIContent.none);

            //重新设置为原来的层级
            EditorGUI.indentLevel = index;

            EditorGUI.EndProperty();
        }
    }
}