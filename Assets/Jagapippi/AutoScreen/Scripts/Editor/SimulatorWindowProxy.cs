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
        private static readonly Type SimulatorWindow;
        private static readonly FieldInfo SimulationStateInfo;
        private static readonly FieldInfo PlayModeViewsFieldInfo;

        // NOTE: なぜか宣言時に初期化するとアセンブリが参照できない
        static SimulatorWindowProxy()
        {
            SimulatorWindow = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetName().Name == "Unity.DeviceSimulator.Editor")
                .Select(assembly => assembly.GetType("Unity.DeviceSimulator.SimulatorWindow"))
                .First();

            SimulationStateInfo = SimulatorWindow.GetField("m_State", BindingFlags.Instance | BindingFlags.NonPublic);

            PlayModeViewsFieldInfo = Assembly.Load("UnityEditor.dll")
                .GetType("UnityEditor.PlayModeView")
                .GetField("s_PlayModeViews", BindingFlags.Static | BindingFlags.NonPublic);

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

        internal static EditorWindow instance => EditorWindow.GetWindow(SimulatorWindow, false, "Simulator", false);
        public static bool isOpen { get; private set; }
        public static bool hasFocus => (isOpen && EditorWindow.focusedWindow && (EditorWindow.focusedWindow.GetType() == SimulatorWindow));

        public static SimulationState state => (SimulationState) SimulationStateInfo.GetValue(instance);
        public static bool enabled => isOpen && (state == SimulationState.Enabled);
    }

    public enum SimulationState
    {
        Enabled,
        Disabled,
    }
}
#endif