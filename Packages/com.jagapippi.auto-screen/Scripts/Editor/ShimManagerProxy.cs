#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityScreen = UnityEngine.Screen;

namespace Jagapippi.AutoScreen
{
    internal static class ShimManagerProxy
    {
        private const string AssemblyName = "UnityEditor.DeviceSimulatorModule";

        private static readonly Type ShimManagerType = Assembly.Load("UnityEngine.dll").GetType("UnityEngine.ShimManager");
        private static readonly FieldInfo ActiveScreenShimFieldInfo = ShimManagerType.GetField("s_ActiveScreenShim", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly PropertyInfo WidthPropertyInfo;
        private static readonly PropertyInfo HeightPropertyInfo;

        static ShimManagerProxy()
        {
            var screenSimulationType = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetName().Name == AssemblyName)
                .Select(assembly => assembly.GetType("UnityEditor.DeviceSimulation.ScreenSimulation"))
                .First();

            WidthPropertyInfo = screenSimulationType.GetProperty("Width");
            HeightPropertyInfo = screenSimulationType.GetProperty("Height");
        }

        public static object GetActiveScreenShim() => ActiveScreenShimFieldInfo.GetValue(null);

        // NOTE: ScreenSimulation#widthの値がおかしい場合があるのでScreenSimulation#Widthを参照する
        public static int width
        {
            get
            {
                var activeScreenShim = GetActiveScreenShim();
                if (activeScreenShim == null) return UnityScreen.width;
                return (int)WidthPropertyInfo.GetValue(activeScreenShim);
            }
        }

        // NOTE: ScreenSimulation#heightの値がおかしい場合があるのでScreenSimulation#Heightを参照する
        public static int height
        {
            get
            {
                var activeScreenShim = GetActiveScreenShim();
                if (activeScreenShim == null) return UnityScreen.height;
                return (int)HeightPropertyInfo.GetValue(activeScreenShim);
            }
        }
    }
}
#endif
