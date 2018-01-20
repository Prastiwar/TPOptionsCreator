using UnityEngine;
using UnityEditor;
using TP_Menu;
using UnityEditor.SceneManagement;

namespace TP_MenuEditor
{
    [InitializeOnLoad]
    public class TPMenuDesigner : EditorWindow
    {
        public static TPMenuDesigner window;
        static string currentScene;

        [MenuItem("TP_Creator/TP_MenuCreator")]
        public static void OpenWindow()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.Log("You can't change Menu Designer runtime!");
                return;
            }
            window = (TPMenuDesigner)GetWindow(typeof(TPMenuDesigner));
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
                if (TPMenuToolsWindow.window)
                    TPMenuToolsWindow.window.Close();
                if (window)
                    window.Close();
            }
        }

        public static TPMenuGUIData EditorData;
        public static TPMenuCreator MenuCreator;
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

            if(MenuCreator)
                creator = new SerializedObject(MenuCreator);
        }

        void InitEditorData()
        {
            EditorData = AssetDatabase.LoadAssetAtPath(
                   "Assets/TP_Creator/TP_MenuCreator/EditorResources/MenuEditorGUIData.asset",
                   typeof(TPMenuGUIData)) as TPMenuGUIData;
            
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
                      "Assets/TP_Creator/TP_MenuCreator/EditorResources/TPMenuGUISkin.guiskin",
                      typeof(GUISkin)) as GUISkin;

            if (EditorData.MenuPrefab == null)
                EditorData.MenuPrefab = AssetDatabase.LoadAssetAtPath(
                    "Assets/TP_Creator/TP_MenuCreator/EditorResources/MenuCanvas.prefab",
                    typeof(GameObject)) as GameObject;

            EditorUtility.SetDirty(EditorData);
        }

        void CreateEditorData()
        {
            TPMenuGUIData newEditorData = ScriptableObject.CreateInstance<TPMenuGUIData>();
            AssetDatabase.CreateAsset(newEditorData, "Assets/TP_Creator/TP_MenuCreator/EditorResources/MenuEditorGUIData.asset");
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
            if (MenuCreator == null)
            {
                MenuCreator = FindObjectOfType<TPMenuCreator>();

                if (MenuCreator != null)
                    UpdateManager();
            }
        }

        void OnGUI()
        {
            if (EditorApplication.isPlaying)
            {
                if (TPMenuToolsWindow.window)
                    TPMenuToolsWindow.window.Close();
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
            GUILayout.Label("TP Menu Creator - Manage your Main Menu!", skin.GetStyle("HeaderLabel"));
            GUILayout.EndArea();
        }

        void DrawManager()
        {
            GUILayout.BeginArea(managerSection);
            GUILayout.Label("Menu Manager - Core", skin.box);

            if (MenuCreator == null)
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
                GameObject go = (new GameObject("TP_MenuManager", typeof(TPMenuCreator)));
                MenuCreator = go.GetComponent<TPMenuCreator>();
                UpdateManager();
                Debug.Log("Menu Manager created!");
            }

            if (GUILayout.Button("Initialize Exist Manager", skin.button, GUILayout.Height(60)))
                existManager = !existManager;

            if (existManager)
                MenuCreator = EditorGUILayout.ObjectField(MenuCreator, typeof(TPMenuCreator), true,
                    GUILayout.Height(30)) as TPMenuCreator;
        }

        void ResetManager()
        {
            if (GUILayout.Button("Reset Manager", skin.button, GUILayout.Height(45)))
                MenuCreator = null;
        }

        void SpawnEmpty()
        {
            if (GUILayout.Button("Spawn empty Menu", skin.button, GUILayout.Height(50)))
            {
                if (EditorData.MenuPrefab == null)
                {
                    Debug.LogError("There is no menu prefab in EditorGUIData file!");
                    return;
                }
                Instantiate(EditorData.MenuPrefab);
                Debug.Log("Menu example Created");
            }
        }

        public static void UpdateManager()
        {
            if(MenuCreator.OptionsLayout != null)
                MenuCreator.OptionsLayout.Refresh();
            if(creator != null)
                creator.ApplyModifiedProperties();
            if(MenuCreator)
                MenuCreator.Refresh();
        }

        void DrawTools()
        {

            GUILayout.BeginArea(toolSection);
            GUILayout.Label("Menu Manager - Tools", skin.box);

            if (MenuCreator == null)
            {
                GUILayout.EndArea();
                return;
            }

            if (GUILayout.Button("Options Menu", skin.button, GUILayout.Height(60)))
            {
                TPMenuToolsWindow.OpenToolWindow(TPMenuToolsWindow.ToolEnum.Options);
            }
            //if (GUILayout.Button("Observers", skin.button, GUILayout.Height(60)))
            //{
            //    TPMenuToolsWindow.OpenToolWindow(TPMenuToolsWindow.ToolEnum.Observers);
            //}
            //if (GUILayout.Button("Layout", skin.button, GUILayout.Height(60)))
            //{
            //    TPMenuToolsWindow.OpenToolWindow(TPMenuToolsWindow.ToolEnum.Layout);
            //}
            GUILayout.EndArea();
        }

    }
}