using HookSystem.Runtime.Managers;
using UnityEngine;

namespace HookSystem.Runtime.Behaviours
{
    public abstract class HookableMono : MonoBehaviour
    {
        [SerializeField] protected float hookUpdateInterval = -1f;

        protected virtual void Awake() => HookSystemProvider.Register(this, hookUpdateInterval);
        protected virtual void OnDestroy() => HookSystemProvider.Unregister(this);
    }
}