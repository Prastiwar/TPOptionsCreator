using UnityEditor;
using TP_Options;

namespace TP_OptionsEditor
{
    [CustomEditor(typeof(TPOptionsLayout))]
    public class TPOptionsLayoutEditor : ScriptlessOptionsEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script that managing options layout");

            OpenCreator();
        }
    }
}