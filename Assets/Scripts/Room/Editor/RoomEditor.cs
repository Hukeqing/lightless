using System;
using UnityEditor;
using UnityEngine;

namespace Room.Editor
{
    [CustomEditor(typeof(Room))]
    public class RoomEditor : UnityEditor.Editor
    {
        private Room _this;

        private void OnEnable()
        {
            _this = target as Room;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Add a Point"))
            {
                _this.AddPoint();
            }
        }
    }
}