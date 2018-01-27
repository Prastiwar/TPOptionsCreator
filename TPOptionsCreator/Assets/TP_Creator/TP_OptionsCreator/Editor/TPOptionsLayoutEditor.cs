using UnityEditor;
using TP_Options;

namespace TP_OptionsEditor
{
    [CustomEditor(typeof(TPOptionsLayout))]
    internal class TPOptionsLayoutEditor : ScriptlessOptionsEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script that managing options layout");

            if (TPOptionsCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}