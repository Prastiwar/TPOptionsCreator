using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TP_Menu;

namespace TP_MenuEditor
{
    [CustomEditor(typeof(TPMenuCreator))]
    public class TPMenuCreatorEditor : ScriptlessMenuEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script managing your menu's");
            OpenCreator();
        }
    }
}