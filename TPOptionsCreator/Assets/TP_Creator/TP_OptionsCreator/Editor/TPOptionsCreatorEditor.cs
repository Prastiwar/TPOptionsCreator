using UnityEditor;
using TP_Options;

namespace TP_OptionsEditor
{
    [CustomEditor(typeof(TPOptionsCreator))]
    internal class TPOptionsCreatorEditor : ScriptlessOptionsEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script managing your menu's");

            if (TPOptionsCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}