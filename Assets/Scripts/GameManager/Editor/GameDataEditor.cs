using UnityEditor;
using UnityEngine;

namespace GameManager.Editor
{
    [CustomEditor(typeof(GameDataManager))]
    public class GameDataEditor : UnityEditor.Editor
    {
        private GameDataManager _this;

        private void OnEnable()
        {
            _this = target as GameDataManager;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Sort"))
            {
                _this.Sort();
            }
        }
    }
}