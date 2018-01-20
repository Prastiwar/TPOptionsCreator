using UnityEngine;
using UnityEditor;
using TP_Menu;
using UnityEditor.SceneManagement;

namespace TP_MenuEditor
{
    [InitializeOnLoad]
    public class TPMenuToolsWindow : EditorWindow
    {
        public static TPMenuToolsWindow window;
        public enum ToolEnum
        {
            MainMenu,
            Options
        }

        static ToolEnum tool;

        enum Options
        {
            Resolution,
            Quality,
            Aliasing,
            ShadowQuality,
            Shadow,
            Fullscreen,
            VSync,
            FXVolume,
            MusicVolume
        }
        //string[] enumNamesList = System.Enum.GetNames(typeof(TPTooltipObserver.ToolTipType));

        SerializedObject OptionsLayout;
        SerializedProperty _OptionsLayout;
        SerializedProperty layoutResDrop;
        SerializedProperty layoutQualDrop;
        SerializedProperty layoutAliasingDrop;
        SerializedProperty layoutShadowQualDrop;
        SerializedProperty layoutShadowDrop;
        SerializedProperty layoutFullScreen;
        SerializedProperty layoutVSync;
        SerializedProperty layoutFXSlider;
        SerializedProperty layoutMusicSlider;
        SerializedProperty layoutFXText;
        SerializedProperty layoutMusicText;
        SerializedProperty layoutAudioMixer;

        //GUIContent content = new GUIContent("You can drag there multiple observers   |  Size");

        Texture2D mainTexture;
        Texture2D tooltipTexture;
        Texture2D previewTexture;

        Vector2 scrollPos = Vector2.zero;
        Vector2 textureVec;

        Rect mainRect;

        bool[] booleans = new bool[9];
        bool canChange;

        static float windowSize = 515;
        static string currentScene;

        public static void OpenToolWindow(ToolEnum _tool)
        {
            if (window != null)
                window.Close();

            tool = _tool;
            window = (TPMenuToolsWindow)GetWindow(typeof(TPMenuToolsWindow));

            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;

            window.minSize = new Vector2(windowSize, windowSize);
            window.maxSize = new Vector2(windowSize, windowSize);
            window.Show();
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if (TPMenuDesigner.window)
                    TPMenuDesigner.window.Close();
                if (window)
                    window.Close();
            }
        }

        void OnEnable()
        {
            InitTextures();

            FindLayoutProperties();
        }

        void FindLayoutProperties()
        {
            if (TPMenuDesigner.MenuCreator.OptionsLayout)
                OptionsLayout = new SerializedObject(TPMenuDesigner.MenuCreator.OptionsLayout);
            _OptionsLayout = TPMenuDesigner.creator.FindProperty("OptionsLayout");

            if (OptionsLayout != null)
                canChange = true;
            else
                return;

            layoutResDrop = OptionsLayout.FindProperty("resDropdown");
            layoutQualDrop = OptionsLayout.FindProperty("qualityDropdown");
            layoutAliasingDrop = OptionsLayout.FindProperty("aliasingDropdown");
            layoutShadowQualDrop = OptionsLayout.FindProperty("shadowQualDropdown");
            layoutShadowDrop = OptionsLayout.FindProperty("shadowDropdown");
            layoutFullScreen = OptionsLayout.FindProperty("fullscreenToggle");
            layoutVSync = OptionsLayout.FindProperty("vSyncToggle");
            layoutFXSlider = OptionsLayout.FindProperty("fxSlider");
            layoutMusicSlider = OptionsLayout.FindProperty("musicSlider");
            layoutFXText = OptionsLayout.FindProperty("mixerFXText");
            layoutMusicText = OptionsLayout.FindProperty("mixerMusicText");
            layoutAudioMixer = OptionsLayout.FindProperty("AudioMixer");
        }

        void InitTextures()
        {
            Color color = new Color(0.19f, 0.19f, 0.19f);
            mainTexture = new Texture2D(1, 1);
            mainTexture.SetPixel(0, 0, color);
            mainTexture.Apply();
        } 

        void OnGUI()
        {
            mainRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(mainRect, mainTexture);
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(window.position.width), GUILayout.Height(window.position.height));
            DrawTool();
            GUILayout.EndScrollView();
        }

        void DrawTool()
        {
            switch (tool)
            {
                case ToolEnum.MainMenu:
                    DrawMenuTool();
                    break;
                case ToolEnum.Options:
                    DrawOptionsTool();
                    break;
                default:
                    break;
            }
        }

        void Check(SerializedProperty list, int index)
        {
            int length = list.arraySize;
            for (int i = 0; i < length; i++)
            {
                if (i == index)
                    continue;
                if (list.GetArrayElementAtIndex(index).objectReferenceValue == list.GetArrayElementAtIndex(i).objectReferenceValue)
                {
                    list.GetArrayElementAtIndex(i).objectReferenceValue = null;
                }
            }
        }

        void RemoveAsset(SerializedProperty list, int index)
        {
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                if (list.GetArrayElementAtIndex(index).objectReferenceValue != null || index == list.arraySize - 1)
                {
                    if (list.GetArrayElementAtIndex(index).objectReferenceValue != null)
                    {
                        //TPTooltipObserver script = (list.GetArrayElementAtIndex(index).objectReferenceValue as GameObject).GetComponent<TPTooltipObserver>();
                        //DestroyImmediate(script);
                        list.GetArrayElementAtIndex(index).objectReferenceValue = null;
                    }
                    list.DeleteArrayElementAtIndex(index);
                }
            }
        }

        void AddObserver()
        {
            //observerOBJList.arraySize++;
            //observerOBJList.serializedObject.ApplyModifiedProperties();
            TPMenuDesigner.UpdateManager();
        }

        void EditAsset(SerializedProperty list, int index)
        {
            if (list.GetArrayElementAtIndex(index).objectReferenceValue != null)
                if (GUILayout.Button("Edit", GUILayout.Width(35)))
                {
                    AssetDatabase.OpenAsset(list.GetArrayElementAtIndex(index).objectReferenceValue);
                }
        }

        void DrawMenuTool()
        {
            
        }

        void DrawOptionsToggleButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Resolution Setter", GUILayout.Width(250)))
                SetBool(0);
            if (GUILayout.Button("Toggle Quality Setter", GUILayout.Width(250)))
                SetBool(1);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Antialiasing Setter", GUILayout.Width(250)))
                SetBool(2);
            if (GUILayout.Button("Toggle Shadow quality Setter", GUILayout.Width(250)))
                SetBool(3);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Shadow Setter", GUILayout.Width(250)))
                SetBool(4);
            if (GUILayout.Button("Toggle Fullscreen Setter", GUILayout.Width(250)))
                SetBool(5);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle VSync Setter", GUILayout.Width(166)))
                SetBool(6);
            if (GUILayout.Button("Toggle FX volume Setter", GUILayout.Width(166)))
                SetBool(7);
            if (GUILayout.Button("Toggle Music volume Setter", GUILayout.Width(166)))
                SetBool(8);
            EditorGUILayout.EndHorizontal();
        }

        void DrawOptionsTool()
        {
            _OptionsLayout.serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.LabelField("Put there your Options Menu Prefab", TPMenuDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(_OptionsLayout, GUIContent.none);
            if (GUI.changed)
            {
                _OptionsLayout.serializedObject.ApplyModifiedProperties();
                FindLayoutProperties();
            }
            if (!canChange)
                return;

            DrawOptionsToggleButtons();

            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.Space();
            if (booleans[0])
            {
                EditorGUILayout.LabelField("Resolution Dropdown", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutResDrop, new GUIContent("Resolutio Dropdown"));
            }
            EditorGUILayout.Space();
            if (booleans[1])
            {
                EditorGUILayout.LabelField("Quality Dropdown", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutQualDrop, new GUIContent("QualityLevel Dropdown"));
            }
            EditorGUILayout.Space();
            if (booleans[2])
            {
                EditorGUILayout.LabelField("Antialiasing Dropdown", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutAliasingDrop, new GUIContent("Antialiasing Dropdown"));
            }
            EditorGUILayout.Space();
            if (booleans[3])
            {
                EditorGUILayout.LabelField("Shadow quality Dropdown", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutShadowQualDrop, new GUIContent("ShadowQuality Dropdown"));
            }
            EditorGUILayout.Space();
            if (booleans[4])
            {
                EditorGUILayout.LabelField("Shadow Dropdown", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutShadowDrop, new GUIContent("Shadow Dropdown"));
            }
            EditorGUILayout.Space();
            if (booleans[5])
            {
                EditorGUILayout.LabelField("Fullscreen Toggle", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutFullScreen, new GUIContent("Fullscreen Toggle"));
            }
            EditorGUILayout.Space();
            if (booleans[6])
            {
                EditorGUILayout.LabelField("VSync Toggle", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutVSync, new GUIContent("VSync Toggle"));
            }
            EditorGUILayout.Space();
            if (booleans[7] || booleans[8])
            {
                EditorGUILayout.LabelField("Audio Mixer", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutAudioMixer, new GUIContent("Audio Mixer for Volumes"));
            }
            EditorGUILayout.Space();
            if (booleans[7])
            {
                EditorGUILayout.LabelField("FX volume Slider", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutFXSlider, new GUIContent("FXVolume Slider"));
                EditorGUILayout.PropertyField(layoutFXText, new GUIContent("FX's property name"));
            }
            EditorGUILayout.Space();
            if (booleans[8])
            {
                EditorGUILayout.LabelField("Music volume Slider", TPMenuDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutMusicSlider, new GUIContent("MusicVolume Slider"));
                EditorGUILayout.PropertyField(layoutMusicText, new GUIContent("Musics's property name"));
            }


            EditorGUILayout.EndVertical();
        }

        void SetBool(int index)
        {
            booleans[index] = !booleans[index];
        }

        void Update()
        {
            if (EditorApplication.isCompiling)
                this.Close();
        }
    }

}