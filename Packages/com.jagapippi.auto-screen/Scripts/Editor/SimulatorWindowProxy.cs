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
        private const string AssemblyName = "UnityEditor.DeviceSimulatorModule";
        private const string SimulatorWindowTypeName = "UnityEditor.DeviceSimulation.SimulatorWindow";

        private static readonly Type SimulatorWindow;
        private static readonly FieldInfo PlayModeViewsFieldInfo;
#if !UNITY_2021_1
        private static readonly MethodInfo RepaintImmediatelyMethodInfo;
#endif

        // NOTE: なぜか宣言時に初期化するとアセンブリが参照できない
        static SimulatorWindowProxy()
        {
            SimulatorWindow = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetName().Name == AssemblyName)
                .Select(assembly => assembly.GetType(SimulatorWindowTypeName))
                .First();

            PlayModeViewsFieldInfo = Assembly.Load("UnityEditor.dll")
                .GetType("UnityEditor.PlayModeView")
                .GetField("s_PlayModeViews", BindingFlags.Static | BindingFlags.NonPublic);

#if !UNITY_2021_1
            RepaintImmediatelyMethodInfo = typeof(EditorWindow).GetMethod("RepaintImmediately", BindingFlags.Instance | BindingFlags.NonPublic);
#endif

            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            var playModeViews = (IEnumerable)PlayModeViewsFieldInfo.GetValue(null);

            isOpen = false;

            foreach (var playModeView in playModeViews)
            {
                if ((UnityEngine.Object)playModeView == null) continue;
                if (playModeView.GetType() != SimulatorWindow) continue;

                isOpen = true;
                break;
            }

            if (_shouldBeRepaint) Repaint();
        }

        public static bool isOpen { get; private set; }
        public static bool hasFocus => (isOpen && EditorWindow.focusedWindow && (EditorWindow.focusedWindow.GetType() == SimulatorWindow));

        private static bool _shouldBeRepaint;
#if !UNITY_2021_1
        public static void RepaintWithDelay() => _shouldBeRepaint = true;
#endif

        public static void Repaint()
        {
            if (isOpen == false) return;

            var playModeViews = (IEnumerable)PlayModeViewsFieldInfo.GetValue(null);

            foreach (EditorWindow playModeView in playModeViews)
            {
                if (playModeView == null) continue;
                if (playModeView.GetType() != SimulatorWindow) continue;

#if UNITY_2021_1
                playModeView.Repaint();
#else
                RepaintImmediately(playModeView);
#endif
            }
        }

#if !UNITY_2021_1
        private static void RepaintImmediately(EditorWindow window)
        {
            if (window == null) return;

            RepaintImmediatelyMethodInfo.Invoke(window, null);
        }
#endif
    }
}
#endif
