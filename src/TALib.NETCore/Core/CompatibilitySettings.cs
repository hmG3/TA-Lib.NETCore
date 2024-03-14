namespace TALib;

public static partial class Core
{
    public static class CompatibilitySettings
    {
        private static CompatibilityMode _compatibilityMode = CompatibilityMode.Default;

        public static CompatibilityMode Get() => _compatibilityMode;

        public static void Set(CompatibilityMode mode) => _compatibilityMode = mode;
    }
}
