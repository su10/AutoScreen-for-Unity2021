#if UNITY_EDITOR
using UnityEditor;

namespace Jagapippi.AutoScreen
{
    public static class AutoScreenSettings
    {
        public static readonly string assetLabel = "AutoScreen";

        public static readonly DeviceFrame deviceFrame = new DeviceFrame();

        public class DeviceFrame
        {
            private readonly string EnabledKey = string.Join(".", typeof(DeviceFrame).FullName, nameof(enabled));

            public bool enabled
            {
                get { return EditorPrefs.GetBool(EnabledKey, true); }
                set { EditorPrefs.SetBool(EnabledKey, value); }
            }
        }

        public static readonly SafeAreaBorder safeAreaBorder = new SafeAreaBorder();

        public class SafeAreaBorder
        {
            private readonly string EnabledKey = string.Join(".", typeof(SafeAreaBorder).FullName, nameof(enabled));

            public bool enabled
            {
                get { return EditorPrefs.GetBool(EnabledKey, true); }
                set { EditorPrefs.SetBool(EnabledKey, value); }
            }
        }
    }
}
#endif