#if !UNITY_2021_1 && !UNITY_2021_2_0 && !UNITY_2021_2_1 && !UNITY_2021_2_2 && !UNITY_2021_2_3 && !UNITY_2021_2_4 && !UNITY_2021_2_5 && !UNITY_2021_2_6 && !UNITY_2021_2_7
#define UNITY_2021_2_8_OR_NEWER
#endif

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
#if UNITY_2021_2_8_OR_NEWER
        private const string WidthKey = "width";
        private const string HeightKey = "height";
#else
        private const string WidthKey = "Width";
        private const string HeightKey = "Height";
#endif

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

            WidthPropertyInfo = screenSimulationType.GetProperty(WidthKey);
            HeightPropertyInfo = screenSimulationType.GetProperty(HeightKey);
        }

        public static object GetActiveScreenShim()
        {
            var activeScreenShim = ActiveScreenShimFieldInfo.GetValue(null);
            if (activeScreenShim is System.Collections.IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    activeScreenShim = enumerator.Current;
                    if (activeScreenShim.GetType().Name == "ScreenSimulation") break;
                }
            }
            return activeScreenShim;
        }

        // NOTE: ScreenSimulation#widthの値がおかしい場合があるのでScreenSimulation#Widthを参照する
        public static int width
        {
            get
            {
                var activeScreenShim = GetActiveScreenShim();
                if (activeScreenShim == null) return UnityScreen.width;
                return activeScreenShim.GetType().GetProperty(WidthKey).GetValue(activeScreenShim) as int? ?? UnityScreen.width;
            }
        }

        // NOTE: ScreenSimulation#heightの値がおかしい場合があるのでScreenSimulation#Heightを参照する
        public static int height
        {
            get
            {
                var activeScreenShim = GetActiveScreenShim();
                if (activeScreenShim == null) return UnityScreen.height;
                return activeScreenShim.GetType().GetProperty(HeightKey).GetValue(activeScreenShim) as int? ?? UnityScreen.height;
            }
        }
    }
}
#endif
