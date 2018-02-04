using UnityEditor;
using TP.Options;

namespace TP.OptionsEditor
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