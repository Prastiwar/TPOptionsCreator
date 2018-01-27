using UnityEngine;
using UnityEditor;
using TP_Options;

namespace TP_OptionsEditor
{
    [CustomEditor(typeof(TPOptionsGUIData))]
    internal class TPOptionsGUIDataEditor : ScriptlessOptionsEditor
    {
        TPOptionsGUIData TPMenuData;

        void OnEnable()
        {
            TPMenuData = (TPOptionsGUIData)target;
            if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Container for editor data");
            if (!TPOptionsCreator.DebugMode)
                return;

            EditorGUILayout.LabelField("GUI Skin");
            TPMenuData.GUISkin =
                (EditorGUILayout.ObjectField(TPMenuData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Empty Options Menu Prefab");
            TPMenuData.OptionsPrefab = (EditorGUILayout.ObjectField(TPMenuData.OptionsPrefab, typeof(GameObject), true) as GameObject);
        }
    }
}