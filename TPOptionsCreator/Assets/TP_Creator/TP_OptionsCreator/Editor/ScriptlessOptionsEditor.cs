using UnityEditor;
using UnityEngine;

namespace TP_OptionsEditor
{
    public class ScriptlessOptionsEditor : Editor
    {
        public readonly string scriptField = "m_Script";

        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

        public void OpenCreator()
        {
            if (GUILayout.Button("Open Menu Manager", GUILayout.Height(30)))
            {
                TPOptionsDesigner.OpenWindow();
            }
        }
    }
}