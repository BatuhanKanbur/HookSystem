using HookSystem.Runtime.Attributes;
using HookSystem.Runtime.Behaviours;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HookSystem.Runtime.Demo
{
    public class HookSystemDemo : HookableMono
    {
        [HookVar("OnHealthChanged")]
        [SerializeField] private int _health = 100;

        private InventoryItem _demoItem;
        private GUIStyle _containerStyle;
        private GUIStyle _headerStyle;
        private GUIStyle _statStyle;
        private GUIStyle _infoStyle;
        private GUIStyle _creditStyle;
        private GUIStyle _linkStyle;

        protected override void Awake()
        {
            base.Awake();
            _demoItem = new InventoryItem();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) _health -= 10;
            if (Input.GetKeyDown(KeyCode.G)) _demoItem.AddGold(50);
            if (Input.GetKeyDown(KeyCode.N))
            {
                int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
                SceneManager.LoadScene(nextScene % SceneManager.sceneCountInBuildSettings);
            }
        }

        private void OnHealthChanged(object oldVal, object newVal)
        {
            Debug.Log($"<color=red>[Mono] Health: {oldVal} -> {newVal}</color>");
        }

        private void InitStyles()
        {
            _containerStyle = new GUIStyle(GUI.skin.box);
            _containerStyle.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.9f));
            _containerStyle.padding = new RectOffset(30, 30, 30, 30);

            _headerStyle = new GUIStyle(GUI.skin.label);
            _headerStyle.fontSize = 36;
            _headerStyle.fontStyle = FontStyle.Bold;
            _headerStyle.normal.textColor = new Color(0.2f, 0.8f, 1f);
            _headerStyle.alignment = TextAnchor.MiddleCenter;

            _statStyle = new GUIStyle(GUI.skin.label);
            _statStyle.fontSize = 28;
            _statStyle.fontStyle = FontStyle.Bold;
            _statStyle.normal.textColor = Color.white;
            _statStyle.richText = true;

            _infoStyle = new GUIStyle(GUI.skin.label);
            _infoStyle.fontSize = 22;
            _infoStyle.fontStyle = FontStyle.Italic;
            _infoStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
            _infoStyle.alignment = TextAnchor.MiddleRight;

            _creditStyle = new GUIStyle(GUI.skin.label);
            _creditStyle.fontSize = 24;
            _creditStyle.alignment = TextAnchor.MiddleCenter;
            _creditStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);

            _linkStyle = new GUIStyle(GUI.skin.label);
            _linkStyle.fontSize = 20;
            _linkStyle.alignment = TextAnchor.MiddleCenter;
            _linkStyle.normal.textColor = new Color(0.4f, 0.6f, 1f);
            _linkStyle.hover.textColor = Color.white;
        }

        private void OnGUI()
        {
            if (_containerStyle == null || _containerStyle.normal.background == null) InitStyles();

            float width = 600;
            float height = 600; 
        
            float x = (Screen.width - width) * 0.5f;
            float y = (Screen.height - height) * 0.5f;

            GUILayout.BeginArea(new Rect(x, y, width, height), _containerStyle);
        
            GUILayout.Label("HOOK SYSTEM DEBUG", _headerStyle);
            GUILayout.Space(20);
        
            DrawDivider();
            GUILayout.Space(20);

            GUILayout.Label($"♥  HEALTH STATUS", _infoStyle);
            GUILayout.Label($"   <color={( _health > 50 ? "#44ff44" : "#ff4444" )}>{_health} HP</color>", _statStyle);
        
            GUILayout.Space(10);

            GUILayout.Label($"$  ECONOMY", _infoStyle);
            GUILayout.Label($"   <color=#ffd700>{_demoItem.Gold} Gold</color>", _statStyle);

            GUILayout.Space(30);
            DrawDivider();
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Label("[SPACE] DAMAGE", _infoStyle);
            GUILayout.Label("[G] Gold", _infoStyle);
            GUILayout.Label("[N] Scene", _infoStyle);
            GUILayout.EndHorizontal();

            GUILayout.Space(30);
            DrawDivider();
            GUILayout.Space(10);

            GUILayout.Label("Batuhan Kanbur", _creditStyle);
            if (GUILayout.Button("www.batuhankanbur.com", _linkStyle))
            {
                Application.OpenURL("https://www.batuhankanbur.com");
            }

            GUILayout.EndArea();
        }

        private void DrawDivider()
        {
            var rect = GUILayoutUtility.GetRect(1f, 2f);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 2), new Color(1, 1, 1, 0.2f));
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i) pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }

    public class InventoryItem : HookableObject
    {
        [HookVar("OnGoldChanged")]
        public int Gold;

        public void AddGold(int amount) => Gold += amount;

        private void OnGoldChanged(object oldVal, object newVal)
        {
            Debug.Log($"<color=yellow>[POCO] Gold: {oldVal} -> {newVal}</color>");
        }
    }
}