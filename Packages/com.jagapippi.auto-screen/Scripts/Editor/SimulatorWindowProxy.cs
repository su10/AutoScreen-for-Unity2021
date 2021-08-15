#if UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Jagapippi.AutoScreen
{
    public static class SimulatorWindowProxy
    {
        private const string SimulatorWindowTypeName =
#if DEVICE_SIMULATOR_3_OR_NEWER
                "UnityEditor.DeviceSimulation.SimulatorWindow"
#else
                "Unity.DeviceSimulator.SimulatorWindow"
#endif
            ;

        private static readonly Type SimulatorWindow;
        private static readonly FieldInfo PlayModeViewsFieldInfo;
#if DEVICE_SIMULATOR_3_OR_NEWER
        private static readonly MethodInfo RestartAllSimulatorsMethodInfo;
#endif

        // NOTE: なぜか宣言時に初期化するとアセンブリが参照できない
        static SimulatorWindowProxy()
        {
            SimulatorWindow = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetName().Name == "Unity.DeviceSimulator.Editor")
                .Select(assembly => assembly.GetType(SimulatorWindowTypeName))
                .First();

            PlayModeViewsFieldInfo = Assembly.Load("UnityEditor.dll")
                .GetType("UnityEditor.PlayModeView")
                .GetField("s_PlayModeViews", BindingFlags.Static | BindingFlags.NonPublic);

#if DEVICE_SIMULATOR_3_OR_NEWER
            RestartAllSimulatorsMethodInfo = SimulatorWindow.GetMethod(nameof(RestartAllSimulators), BindingFlags.Static | BindingFlags.Public);
#endif

            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            var playModeViews = (IEnumerable) PlayModeViewsFieldInfo.GetValue(null);

            isOpen = false;

            foreach (var playModeView in playModeViews)
            {
                if ((UnityEngine.Object) playModeView == null) continue;
                if (playModeView.GetType() != SimulatorWindow) continue;

                isOpen = true;
                break;
            }
        }

        public static bool isOpen { get; private set; }
        public static bool hasFocus => (isOpen && EditorWindow.focusedWindow && (EditorWindow.focusedWindow.GetType() == SimulatorWindow));

        public static void Reload()
        {
            Repaint();
            RestartAllSimulators();
        }

        private static void Repaint()
        {
            if (isOpen == false) return;

            var playModeViews = (IEnumerable) PlayModeViewsFieldInfo.GetValue(null);

            foreach (var playModeView in playModeViews)
            {
                if ((UnityEngine.Object) playModeView == null) continue;
                if (playModeView.GetType() != SimulatorWindow) continue;

                ((EditorWindow) playModeView).Repaint();
            }
        }

        private static void RestartAllSimulators()
        {
#if DEVICE_SIMULATOR_3_OR_NEWER
            RestartAllSimulatorsMethodInfo.Invoke(null, null);
#endif
        }
    }
}
#endif