using UnityEngine;

namespace TP_MenuEditor
{
    public class TPMenuGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        [HideInInspector] public GameObject MenuPrefab;
    }
}