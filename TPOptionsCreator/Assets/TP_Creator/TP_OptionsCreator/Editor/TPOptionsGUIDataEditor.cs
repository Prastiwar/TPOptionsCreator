using UnityEngine;
using UnityEditor;

namespace TP_OptionsEditor
{
    [CustomEditor(typeof(TPOptionsGUIData))]
    public class TPOptionsGUIDataEditor : ScriptlessOptionsEditor
    {
        TPOptionsGUIData TPMenuData;

        void OnEnable()
        {
            TPMenuData = (TPOptionsGUIData)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("GUI Skin");
            TPMenuData.GUISkin =
                (EditorGUILayout.ObjectField(TPMenuData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Options Menu Prefab");
            TPMenuData.OptionsPrefab = (EditorGUILayout.ObjectField(TPMenuData.OptionsPrefab, typeof(GameObject), true) as GameObject);

            if (GUI.changed)
                EditorUtility.SetDirty(TPMenuData);

            serializedObject.ApplyModifiedProperties();
        }
    }
}