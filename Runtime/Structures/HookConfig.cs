using UnityEngine;

namespace HookSystem.Runtime.Structures
{
    [CreateAssetMenu(fileName = "HookConfig", menuName = "HookSystem/Config")]
    public class HookConfig : ScriptableObject
    {
        [Range(0.01f, 2f)]
        public float GlobalTickRate = 0.01f;
        public bool EnableDebugLogs;
    }
}