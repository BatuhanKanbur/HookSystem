using System;
using System.Collections.Generic;
using System.Reflection;
using HookSystem.Runtime.Attributes;
using HookSystem.Runtime.Structures;

namespace HookSystem.Runtime.Managers
{
    internal class HookTracker
    {
        public readonly WeakReference<object> Target;
        private readonly List<HookData> _hooks = new();
        private readonly float _interval;
        private float _timer;

        public HookTracker(object target, float interval)
        {
            Target = new WeakReference<object>(target);
            _interval = interval;
            CacheHooks(target);
        }

        public bool Tick(float dt)
        {
            _timer += dt;
            if (!(_timer >= _interval)) return false;
            _timer = 0;
            return true;
        }

        private void CacheHooks(object target)
        {
            var members = target.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var member in members)
            {
                var attr = member.GetCustomAttribute<HookVar>();
                if (attr == null) continue;
                if (member is not FieldInfo && (member is not PropertyInfo p || !p.CanRead)) continue;

                var data = new HookData { Member = member };
                data.LastValue = data.GetCurrentValue(target);

                if (!string.IsNullOrEmpty(attr.HookMethod))
                {
                    var eventInfo = target.GetType().GetField(attr.HookMethod,
                        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    if (eventInfo != null && typeof(Delegate).IsAssignableFrom(eventInfo.FieldType))
                        data.CallbackEvent = eventInfo.GetValue(target) as Delegate;
                    else
                        data.CallbackMethod = target.GetType().GetMethod(attr.HookMethod,
                            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                }

                _hooks.Add(data);
            }
        }

        public void CheckChanges(object target)
        {
            foreach (var data in _hooks)
            {
                var current = data.GetCurrentValue(target);
                if (Equals(data.LastValue, current)) continue;

                if (data.CallbackEvent != null)
                    data.CallbackEvent.DynamicInvoke(data.LastValue, current);
                else
                    data.CallbackMethod?.Invoke(target, new[] { data.LastValue, current });

                data.LastValue = current;
            }
        }
    }
}