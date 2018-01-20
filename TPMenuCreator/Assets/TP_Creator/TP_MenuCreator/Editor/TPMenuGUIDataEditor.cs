using UnityEngine;
using UnityEditor;

namespace TP_MenuEditor
{
    [CustomEditor(typeof(TPMenuGUIData))]
    public class TPTooltipGUIDataEditor : ScriptlessMenuEditor
    {
        TPMenuGUIData TPMenuData;

        void OnEnable()
        {
            TPMenuData = (TPMenuGUIData)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("GUI Skin");
            TPMenuData.GUISkin =
                (EditorGUILayout.ObjectField(TPMenuData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Menu Prefab");
            TPMenuData.MenuPrefab = (EditorGUILayout.ObjectField(TPMenuData.MenuPrefab, typeof(GameObject), true) as GameObject);

            if (GUI.changed)
                EditorUtility.SetDirty(TPMenuData);

            serializedObject.ApplyModifiedProperties();
        }
    }
}