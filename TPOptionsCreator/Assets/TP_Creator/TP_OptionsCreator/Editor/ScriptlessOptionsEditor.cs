using TP_Options;
using UnityEditor;
using UnityEngine;

namespace TP_OptionsEditor
{
    internal class ScriptlessOptionsEditor : Editor
    {
        public readonly string scriptField = "m_Script";

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

        public void OpenCreator()
        {
            if (TPOptionsCreator.DebugMode)
            {
                if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                    serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
                return;
            }

            if (serializedObject.targetObject.hideFlags != HideFlags.None)
                serializedObject.targetObject.hideFlags = HideFlags.None;

            if (GUILayout.Button("Open Menu Manager", GUILayout.Height(30)))
            {
                TPOptionsDesigner.OpenWindow();
            }
        }
    }
}