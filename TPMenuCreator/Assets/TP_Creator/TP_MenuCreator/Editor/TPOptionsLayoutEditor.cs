using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Menu;

namespace TP_MenuEditor
{
    [CustomEditor(typeof(TPOptionsLayout))]
    public class TPOptionsLayoutEditor : ScriptlessMenuEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script that managing options layout");

            OpenCreator();
        }
    }
}