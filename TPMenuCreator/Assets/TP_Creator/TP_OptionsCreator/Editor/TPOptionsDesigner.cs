using UnityEngine;
using UnityEditor;
using TP_Options;
using UnityEditor.SceneManagement;

namespace TP_OptionsEditor
{
    [InitializeOnLoad]
    public class TPOptionsDesigner : EditorWindow
    {
        public static TPOptionsDesigner window;
        static string currentScene;

        [MenuItem("TP_Creator/TP_OptionsCreator")]
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.Log("You can't change Options Menu Designer runtime!");
                return;
            }
            window = (TPOptionsDesigner)GetWindow(typeof(TPOptionsDesigner));
            currentScene = EditorSceneManager.GetActiveScene().name;
            EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;
            window.minSize = new Vector2(615, 290);
            window.maxSize = new Vector2(615, 290);
            window.Show();
        }

        static void hierarchyWindowChanged()
        {
            if (currentScene != EditorSceneManager.GetActiveScene().name)
            {
                if (TPOptionsToolsWindow.window)
                    TPOptionsToolsWindow.window.Close();
                if (window)
                    window.Close();
            }
        }

        public static TPOptionsGUIData EditorData;
        public static TPOptionsCreator OptionsCreator;
        public static GUISkin skin;

        Texture2D headerTexture;
        Texture2D managerTexture;
        Texture2D toolTexture;

        Rect headerSection;
        Rect managerSection;
        Rect toolSection;

        bool existManager;
        bool toggleChange;

        public static SerializedObject creator;

        void OnEnable()
        {
            InitEditorData();
            InitTextures();
            InitCreator();

            if(OptionsCreator)
                creator = new SerializedObject(OptionsCreator);
        }

        void InitEditorData()
        {
            EditorData = AssetDatabase.LoadAssetAtPath(
                   "Assets/TP_Creator/TP_OptionsCreator/EditorResources/OptionsEditorGUIData.asset",
                   typeof(TPOptionsGUIData)) as TPOptionsGUIData;
            
            if (EditorData == null)
                CreateEditorData();
            else
                CheckGUIData();

            skin = EditorData.GUISkin;
        }

        void CheckGUIData()
        {
            if (EditorData.GUISkin == null)
                EditorData.GUISkin = AssetDatabase.LoadAssetAtPath(
                      "Assets/TP_Creator/TP_OptionsCreator/EditorResources/TPOptionsGUISkin.guiskin",
                      typeof(GUISkin)) as GUISkin;

            if (EditorData.OptionsPrefab == null)
                EditorData.OptionsPrefab = AssetDatabase.LoadAssetAtPath(
                    "Assets/TP_Creator/TP_OptionsCreator/EditorResources/OptionsCanvas.prefab",
                    typeof(GameObject)) as GameObject;

            EditorUtility.SetDirty(EditorData);
        }

        void CreateEditorData()
        {
            TPOptionsGUIData newEditorData = ScriptableObject.CreateInstance<TPOptionsGUIData>();
            AssetDatabase.CreateAsset(newEditorData, "Assets/TP_Creator/TP_OptionsCreator/EditorResources/OptionsEditorGUIData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorData = newEditorData;
            CheckGUIData();
        }

        void InitTextures()
        {
            Color colorHeader = new Color(0.19f, 0.19f, 0.19f);
            Color color = new Color(0.15f, 0.15f, 0.15f);

            headerTexture = new Texture2D(1, 1);
            headerTexture.SetPixel(0, 0, colorHeader);
            headerTexture.Apply();

            managerTexture = new Texture2D(1, 1);
            managerTexture.SetPixel(0, 0, color);
            managerTexture.Apply();

            toolTexture = new Texture2D(1, 1);
            toolTexture.SetPixel(0, 0, color);
            toolTexture.Apply();
        }

        static void InitCreator()
        {
            if (OptionsCreator == null)
            {
                OptionsCreator = FindObjectOfType<TPOptionsCreator>();

                if (OptionsCreator != null)
                    UpdateManager();
            }
        }

        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                if (TPOptionsToolsWindow.window)
                    TPOptionsToolsWindow.window.Close();
                this.Close();
            }
            DrawLayouts();
            DrawHeader();
            DrawManager();
            DrawTools();
        }

        void DrawLayouts()
        {
            headerSection = new Rect(0, 0, Screen.width, 50);
            managerSection = new Rect(0, 50, Screen.width / 2, Screen.height);
            toolSection = new Rect(Screen.width / 2, 50, Screen.width / 2, Screen.height);

            GUI.DrawTexture(headerSection, headerTexture);
            GUI.DrawTexture(managerSection, managerTexture);
            GUI.DrawTexture(toolSection, toolTexture);
        }

        void DrawHeader()
        {
            GUILayout.BeginArea(headerSection);
            GUILayout.Label("TP Options Creator - Manage your Options Menu!", skin.GetStyle("HeaderLabel"));
            GUILayout.EndArea();
        }

        void DrawManager()
        {
            GUILayout.BeginArea(managerSection);
            GUILayout.Label("Options Manager - Core", skin.box);

            if (OptionsCreator == null)
            {
                InitializeManager();
            }
            else
            {
                SpawnEmpty();
                ResetManager();

                if (GUILayout.Button("Refresh and update", skin.button, GUILayout.Height(70)))
                {
                    UpdateManager();
                }
            }

            GUILayout.EndArea();
        }

        void InitializeManager()
        {
            if (GUILayout.Button("Initialize New Manager", skin.button, GUILayout.Height(60)))
            {
                GameObject go = (new GameObject("TP_OptionsManager", typeof(TPOptionsCreator)));
                OptionsCreator = go.GetComponent<TPOptionsCreator>();
                UpdateManager();
                Debug.Log("Options Manager created!");
            }

            if (GUILayout.Button("Initialize Exist Manager", skin.button, GUILayout.Height(60)))
                existManager = !existManager;

            if (existManager)
                OptionsCreator = EditorGUILayout.ObjectField(OptionsCreator, typeof(TPOptionsCreator), true,
                    GUILayout.Height(30)) as TPOptionsCreator;
        }

        void ResetManager()
        {
            if (GUILayout.Button("Reset Manager", skin.button, GUILayout.Height(45)))
                OptionsCreator = null;
        }

        void SpawnEmpty()
        {
            if (GUILayout.Button("Spawn empty Options", skin.button, GUILayout.Height(50)))
            {
                if (EditorData.OptionsPrefab == null)
                {
                    Debug.LogError("There is no options prefab in EditorGUIData file!");
                    return;
                }
                Instantiate(EditorData.OptionsPrefab);
                Debug.Log("Options example Created");
            }
        }

        public static void UpdateManager()
        {
            if(creator != null)
                creator.ApplyModifiedProperties();
            //if(OptionsCreator)
            //    OptionsCreator.Refresh();
        }

        void DrawTools()
        {

            GUILayout.BeginArea(toolSection);
            GUILayout.Label("Options Manager - Tools", skin.box);

            if (OptionsCreator == null)
            {
                GUILayout.EndArea();
                return;
            }

            if (GUILayout.Button("Options Menu Layout", skin.button, GUILayout.Height(60)))
            {
                TPOptionsToolsWindow.OpenToolWindow(/*TPMenuToolsWindow.ToolEnum.Options*/);
            }
            //if (GUILayout.Button("Main Menu", skin.button, GUILayout.Height(60)))
            //{
            //    TPMenuToolsWindow.OpenToolWindow(TPMenuToolsWindow.ToolEnum.MainMenu);
            //}
            GUILayout.EndArea();
        }

    }
}