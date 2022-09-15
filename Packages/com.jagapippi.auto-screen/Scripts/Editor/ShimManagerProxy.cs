#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityScreen = UnityEngine.Screen;

namespace Jagapippi.AutoScreen
{
    internal static class ShimManagerProxy
    {
        private static readonly Type ShimManager = Assembly.Load("UnityEngine.dll").GetType("UnityEngine.ShimManager");
        private static readonly FieldInfo ActiveScreenShimFieldInfo = ShimManager.GetField("s_ActiveScreenShim", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly Type ScreenSimulation;
        private static readonly PropertyInfo WidthPropertyInfo;
        private static readonly PropertyInfo HeightPropertyInfo;

        static ShimManagerProxy()
        {
            ScreenSimulation = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetName().Name == "Unity.DeviceSimulator.Editor")
#if DEVICE_SIMULATOR_3_OR_NEWER
                .Select(assembly => assembly.GetType("UnityEditor.DeviceSimulation.ScreenSimulation"))
#else
                .Select(assembly => assembly.GetType("Unity.DeviceSimulator.ScreenSimulation"))
#endif
                .First();
            WidthPropertyInfo = ScreenSimulation.GetProperty("Width");
            HeightPropertyInfo = ScreenSimulation.GetProperty("Height");
        }

        // NOTE: ScreenSimulation#widthの値がおかしい場合があるのでScreenSimulation#Widthを参照する
        public static int width
        {
            get
            {
                var activeScreenShim = ActiveScreenShimFieldInfo.GetValue(null);
                if (activeScreenShim == null) return UnityScreen.width;
                return (int)WidthPropertyInfo.GetValue(activeScreenShim);
            }
        }

        // NOTE: ScreenSimulation#heightの値がおかしい場合があるのでScreenSimulation#Heightを参照する
        public static int height
        {
            get
            {
                var activeScreenShim = ActiveScreenShimFieldInfo.GetValue(null);
                if (activeScreenShim == null) return UnityScreen.height;
                return (int)HeightPropertyInfo.GetValue(activeScreenShim);
            }
        }
    }
}
#endif
