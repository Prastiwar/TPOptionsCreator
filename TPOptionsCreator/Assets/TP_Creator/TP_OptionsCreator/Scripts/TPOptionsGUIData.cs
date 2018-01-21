using UnityEngine;

namespace TP_OptionsEditor
{
    public class TPOptionsGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        [HideInInspector] public GameObject OptionsPrefab;
    }
}