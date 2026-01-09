using System;
using System.Reflection;

namespace HookSystem.Runtime.Structures
{
    internal class HookData
    {
        public MemberInfo Member;
        public object LastValue;
        public MethodInfo CallbackMethod;
        public Delegate CallbackEvent;

        public object GetCurrentValue(object target) => Member switch
        {
            FieldInfo f => f.GetValue(target),
            PropertyInfo p => p.GetValue(target),
            _ => null
        };
    }
}
