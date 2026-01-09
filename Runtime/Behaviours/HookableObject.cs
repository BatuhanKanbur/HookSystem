using HookSystem.Runtime.Managers;

namespace HookSystem.Runtime.Behaviours
{
    public abstract class HookableObject
    {
        protected HookableObject(float updateInterval = -1f)
        {
            HookSystemProvider.Register(this, updateInterval);
        }

        ~HookableObject() => HookSystemProvider.Unregister(this);
    }
}