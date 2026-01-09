using System;

namespace HookSystem.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HookVar : Attribute
    {
        public string HookMethod { get; }

        public HookVar(string hook = null)
        {
            HookMethod = hook;
        }
    }
}