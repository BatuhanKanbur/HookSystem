using System.Collections.Generic;
using HookSystem.Runtime.Structures;
using UnityEngine;

namespace HookSystem.Runtime.Managers
{
    [DefaultExecutionOrder(-100)]
    public class HookRunner : MonoBehaviour
    {
        private static HookRunner _instance;
        private static bool _isQuitting;
        private readonly List<HookTracker> _trackers = new();
        private HookConfig _config;
        public static HookRunner Instance
        {
            get
            {
                if (_isQuitting) return null;
                if (_instance) return _instance;
                _instance = FindFirstObjectByType<HookRunner>();
                if (_instance) return _instance;
                var go = new GameObject("[HookSystem_Global]");
                _instance = go.AddComponent<HookRunner>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadConfig();
        }
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void LoadConfig()
        {
            _config = Resources.Load<HookConfig>("HookConfig");
            if (!_config) _config = ScriptableObject.CreateInstance<HookConfig>();
        }

        public void Register(object target, float interval)
        {
            var finalInterval = interval > 0 ? interval : _config.GlobalTickRate;
            _trackers.Add(new HookTracker(target, finalInterval));
            if (_config.EnableDebugLogs) Debug.Log($"[HookSystem] Registered: {target.GetType().Name}");
        }

        public void Unregister(object target)
        {
            for (var i = _trackers.Count - 1; i >= 0; i--)
            {
                if (!_trackers[i].Target.TryGetTarget(out var t) || t != target) continue;
                _trackers.RemoveAt(i);
                return;
            }
        }

        private void Update()
        {
            if (_isQuitting) return;
            var deltaTime = Time.unscaledDeltaTime;
            for (var i = _trackers.Count - 1; i >= 0; i--)
            {
                var tracker = _trackers[i];

                if (!tracker.Target.TryGetTarget(out var targetObj) || IsUnityObjectDead(targetObj))
                {
                    _trackers.RemoveAt(i);
                    continue;
                }

                if (tracker.Tick(deltaTime))
                {
                    tracker.CheckChanges(targetObj);
                }
            }
        }

        private bool IsUnityObjectDead(object target)
        {
            if (target is Object uObj)
            {
                return !uObj;
            }
            return false;
        }
    }
}
