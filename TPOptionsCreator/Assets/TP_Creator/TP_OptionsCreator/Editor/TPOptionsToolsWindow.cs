using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace TP.OptionsEditor
{
    [InitializeOnLoad]
    internal class TPOptionsToolsWindow : EditorWindow
    {
        public static TPOptionsToolsWindow window;

            // *** Options Menu *** ///
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
        SerializedProperty layoutTextureDrop;
        SerializedProperty layoutAnisotropicToggle;
        SerializedProperty layoutMusicButton;
        SerializedProperty layoutFXButton;
        SerializedProperty layoutMusicONSprite;
        SerializedProperty layoutFXONSprite;
        SerializedProperty layoutMusicOffSprite;
        SerializedProperty layoutFXOffSprite;
        
        GUIContent content0 = new GUIContent("Resolution Dropdown");
        GUIContent content1 = new GUIContent("QualityLevel Dropdown");
        GUIContent content2 = new GUIContent("Antialiasing Dropdown");
        GUIContent content3 = new GUIContent("ShadowQuality Dropdown");
        GUIContent content4 = new GUIContent("Shadow Dropdown");
        GUIContent content5 = new GUIContent("Fullscreen Toggle");
        GUIContent content6 = new GUIContent("VSync Toggle");
        GUIContent content7 = new GUIContent("Audio Mixer for Volumes");
        GUIContent content8 = new GUIContent("FXVolume Slider");
        GUIContent content9 = new GUIContent("FX's property name");
        GUIContent content10 = new GUIContent("MusicVolume Slider");
        GUIContent content11 = new GUIContent("Musics's property name");
        GUIContent content12 = new GUIContent("Texture's Dropdown");
        GUIContent content13 = new GUIContent("Anisotropic Toggle");
        GUIContent content14 = new GUIContent("Music Button");
        GUIContent content15 = new GUIContent("Music Sprite Off");
        GUIContent content16 = new GUIContent("Music Sprite ON");
        GUIContent content17 = new GUIContent("FX Button");
        GUIContent content18 = new GUIContent("FX Sprite Off");
        GUIContent content19 = new GUIContent("FX Sprite ON");

        // *** Other *** ///
        Texture2D mainTexture;
        Texture2D tooltipTexture;
        Texture2D previewTexture;

        Vector2 scrollPos = Vector2.zero;
        Vector2 textureVec;

        Rect mainRect;

        bool[] booleans = new bool[13];
        bool canChange;

        static float windowSize = 515;
        static string currentScene;

        public static void OpenToolWindow()
        {
            if (window != null)
                window.Close();
            
            window = (TPOptionsToolsWindow)GetWindow(typeof(TPOptionsToolsWindow));

            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;

            window.minSize = new Vector2(windowSize, windowSize);
            window.maxSize = new Vector2(windowSize, windowSize);
            window.Show();
            AssetDatabase.OpenAsset(TPOptionsDesigner.OptionsCreator.OptionsLayout);
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if (TPOptionsDesigner.window)
                    TPOptionsDesigner.window.Close();
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
            if (TPOptionsDesigner.OptionsCreator.OptionsLayout)
                OptionsLayout = new SerializedObject(TPOptionsDesigner.OptionsCreator.OptionsLayout);
            _OptionsLayout = TPOptionsDesigner.creator.FindProperty("OptionsLayout");

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
            layoutTextureDrop = OptionsLayout.FindProperty("textureDropdown");
            layoutAnisotropicToggle = OptionsLayout.FindProperty("anisotropicToggle");
            layoutMusicButton = OptionsLayout.FindProperty("musicButton");
            layoutFXButton = OptionsLayout.FindProperty("fxButton");
            layoutMusicONSprite = OptionsLayout.FindProperty("musicImageOn");
            layoutFXONSprite = OptionsLayout.FindProperty("fxImageOn");
            layoutMusicOffSprite = OptionsLayout.FindProperty("musicImageOff");
            layoutFXOffSprite = OptionsLayout.FindProperty("fxImageOff");
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
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            DrawTool();
            GUILayout.EndScrollView();
        }

        public void DrawTool()
        {
            DrawOptionsTool();
        }

        void DrawOptionsToggleButtons()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle FX volume Set", GUILayout.Width(window.position.width/ 2.05f)))
                SetBool(7);
            if (GUILayout.Button("Toggle Music volume Set", GUILayout.Width(window.position.width / 2.05f)))
                SetBool(8);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Antialiasing Set", GUILayout.Width(window.position.width /3.1f)))
                SetBool(2);
            if (GUILayout.Button("Toggle ShadowQuality Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(3);
            if (GUILayout.Button("Toggle Shadows Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(4);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Texture Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(9);
            if (GUILayout.Button("Toggle Resolution Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(0);
            if (GUILayout.Button("Toggle Quality Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(1);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle VSync Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(6);
            if (GUILayout.Button("Toggle Anisotropic Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(10);
            if (GUILayout.Button("Toggle Fullscreen Set", GUILayout.Width(window.position.width / 3.1f)))
                SetBool(5);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Toggle Music On/Off Set", GUILayout.Width(window.position.width / 2.05f)))
                SetBool(11);
            if (GUILayout.Button("Toggle FX On/Off Set", GUILayout.Width(window.position.width / 2.05f)))
                SetBool(12);
            EditorGUILayout.EndHorizontal();
        }

        void DrawOptionsTool()
        {
            _OptionsLayout.serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.LabelField("Put there your Options Menu Prefab", TPOptionsDesigner.skin.GetStyle("TipLabel"));
            EditorGUILayout.PropertyField(_OptionsLayout, GUIContent.none);
            if (GUI.changed)
            {
                _OptionsLayout.serializedObject.ApplyModifiedProperties();
                FindLayoutProperties();
            }
            if (!canChange)
                return;

            DrawOptionsToggleButtons();

            if (Event.current.type == EventType.DragPerform)
            {
                if (DragAndDrop.objectReferences.Length > 1)
                {
                    return;
                }
                else
                {
                    if ((!PrefabUtility.GetPrefabObject(DragAndDrop.objectReferences[0]) && DragAndDrop.paths.Length < 1) ||
                        (!PrefabUtility.GetPrefabObject(DragAndDrop.objectReferences[0]) && DragAndDrop.paths.Length < 1))
                    {
                        Debug.LogError("You can't drag no-prefab object!");
                        return;
                    }
                }
            }
            EditorGUILayout.BeginVertical();
            
            if (booleans[0])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Resolution Dropdown", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutResDrop, content0);
            }
            if (booleans[1])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quality Dropdown", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutQualDrop, content1);
            }
            if (booleans[2])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Antialiasing Dropdown", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutAliasingDrop, content2);
            }
            if (booleans[3])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Shadow quality Dropdown", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutShadowQualDrop, content3);
            }
            if (booleans[4])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Shadow Dropdown", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutShadowDrop, content4);
            }
            if (booleans[5])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Fullscreen Toggle", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutFullScreen, content5);
            }
            if (booleans[6])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("VSync Toggle", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutVSync, content6);
            }
            if (booleans[7] || booleans[8])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Audio Mixer", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutAudioMixer, content7);
            }
            if (booleans[7])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("FX volume Slider", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutFXSlider, content8);
                EditorGUILayout.PropertyField(layoutFXText, content9);
            }
            if (booleans[8])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Music volume Slider", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutMusicSlider, content10);
                EditorGUILayout.PropertyField(layoutMusicText, content11);
            }
            if (booleans[9])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Texture Quality Dropdown", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutTextureDrop, content12);
            }
            if (booleans[10])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Anisotropic Toggle", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutAnisotropicToggle, content13);
            }
            if (booleans[11])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Music Button", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutMusicButton, content14);
                EditorGUILayout.PropertyField(layoutMusicOffSprite, content15);
                EditorGUILayout.PropertyField(layoutMusicONSprite, content16);
            }
            if (booleans[12])
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("FX Button", TPOptionsDesigner.skin.GetStyle("TipLabel"));
                EditorGUILayout.PropertyField(layoutFXButton, content17);
                EditorGUILayout.PropertyField(layoutFXOffSprite, content18);
                EditorGUILayout.PropertyField(layoutFXONSprite, content19);
            }

            if (GUI.changed)
                OptionsLayout.ApplyModifiedProperties();

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