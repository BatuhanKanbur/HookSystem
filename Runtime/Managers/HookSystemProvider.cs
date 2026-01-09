namespace HookSystem.Runtime.Managers
{
    public static class HookSystemProvider
    {
        private static HookRunner Runner => HookRunner.Instance;

        public static void Register(object target, float interval = -1)
        {
            if (!Runner) return;
            Runner.Register(target, interval);
        }

        public static void Unregister(object target)
        {
            if (!Runner) return;
            Runner.Unregister(target);
        }
    }
}