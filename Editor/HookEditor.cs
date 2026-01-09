using System.IO;
using HookSystem.Runtime.Structures;
using UnityEditor;
using UnityEngine;

namespace HookSystem.Editor
{
    public class HookSettingsWindow : EditorWindow
    {
        private const string CONFIG_PATH = "Assets/Resources/HookConfig.asset";
        private const string FIRST_OPEN_KEY = "HookSystem_FirstOpen";
    
        private HookConfig _config;
        private readonly Vector2 _windowSize = new(450, 650);

        [InitializeOnLoadMethod]
        private static void InitOnLoad()
        {
            if (EditorPrefs.HasKey(FIRST_OPEN_KEY)) return;
            EditorPrefs.SetBool(FIRST_OPEN_KEY, true);
            Open();
        }

        [MenuItem("Tools/Hook System/Settings")]
        public static void Open()
        {
            var window = GetWindow<HookSettingsWindow>("Hook Settings");
            window.minSize = new Vector2(450, 650);
            window.maxSize = new Vector2(450, 650);
            window.Show();
        }

        private void OnEnable()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            _config = AssetDatabase.LoadAssetAtPath<HookConfig>(CONFIG_PATH);
            if (!_config)
            {
                if (!Directory.Exists("Assets/Resources"))
                    Directory.CreateDirectory("Assets/Resources");

                _config = CreateInstance<HookConfig>();
                AssetDatabase.CreateAsset(_config, CONFIG_PATH);
                AssetDatabase.SaveAssets();
            }
        }

        private void OnGUI()
        {
            if (!_config) LoadConfig();
            var headerRect = EditorGUILayout.GetControlRect(false, 70);
            DrawGradientRect(headerRect, new Color(0.1f, 0.5f, 0.9f, 1f), new Color(0.2f, 0.7f, 1f, 1f));
            var headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 22,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(headerRect, "Hook System", headerStyle);
            EditorGUILayout.Space();
            var descStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14
            };
            EditorGUILayout.LabelField("Variable Tracking & Event Polling Solution", descStyle);
            EditorGUILayout.Space(5);
            var devStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 15
            };

            var quoteStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                wordWrap = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 13
            };
            EditorGUILayout.LabelField(
                "\"Engineering is the art of directing the great sources of power in nature for the use and convenience of man.\"",
                quoteStyle);
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Batuhan Kanbur", devStyle);
            EditorGUILayout.Space(5);
            var linkStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                normal = { textColor = Color.cyan },
                hover = { textColor = Color.blue }
            };
            var linkRect = EditorGUILayout.GetControlRect();
            GUI.Label(linkRect, "www.batuhankanbur.com", linkStyle);
            if (Event.current.type == EventType.MouseDown && linkRect.Contains(Event.current.mousePosition))
            {
                Application.OpenURL("https://www.batuhankanbur.com");
            }

            EditorGUILayout.Space(15);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("System Optimization", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Recommended settings for stable hook operations.", EditorStyles.miniLabel);
            EditorGUILayout.Space(5);
        
            bool runInBackground = PlayerSettings.runInBackground;
            string statusIcon = runInBackground ? "✓" : "✕";
            Color statusColor = runInBackground ? Color.green : Color.red;
        
            var statusStyle = new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = statusColor } };
            EditorGUILayout.LabelField($"  {statusIcon} Run In Background: {(runInBackground ? "Enabled" : "Disabled")}", statusStyle);
        
            if (!runInBackground)
            {
                EditorGUILayout.Space(5);
                if (GUILayout.Button("Fix Project Settings"))
                {
                    PlayerSettings.runInBackground = true;
                    Debug.Log("<color=green>[HookSystem] Run In Background enabled.</color>");
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(15);

            EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
        
            EditorGUI.BeginChangeCheck();
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space(5);
        
            _config.GlobalTickRate = EditorGUILayout.Slider("Global Tick Rate", _config.GlobalTickRate, 0.01f, 2f);
            EditorGUILayout.LabelField("Interval in seconds for the global update loop.", EditorStyles.miniLabel);
        
            EditorGUILayout.Space(10);
        
            _config.EnableDebugLogs = EditorGUILayout.Toggle("Enable Debug Logs", _config.EnableDebugLogs);
            EditorGUILayout.LabelField("Show console logs when objects register/unregister.", EditorStyles.miniLabel);

            EditorGUILayout.Space(5);
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_config);
            }

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Save Configuration", GUILayout.Height(35)))
            {
                EditorUtility.SetDirty(_config);
                AssetDatabase.SaveAssets();
                Debug.Log("<color=cyan>[HookSystem] Configuration Saved.</color>");
            }
        
            if (GUILayout.Button("Open Config File", GUILayout.Height(25)))
            {
                Selection.activeObject = _config;
                EditorGUIUtility.PingObject(_config);
            }
        }

        private void DrawGradientRect(Rect pos, Color topColor, Color bottomColor)
        {
            var tex = new Texture2D(1, 2);
            tex.SetPixels(new[] { bottomColor, topColor });
            tex.Apply();
            GUI.DrawTexture(pos, tex);
            DestroyImmediate(tex);
        }
    }
}