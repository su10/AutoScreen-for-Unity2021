#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Jagapippi.AutoScreen
{
    public static class SimulatorWindowEvent
    {
        public static event Action onOpen;
        public static event Action onClose;
        public static event Action onFocus;
        public static event Action onLostFocus;

        private static bool _isOpen;
        private static bool _hasFocus;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            _isOpen = SimulatorWindowProxy.isOpen;
            _hasFocus = SimulatorWindowProxy.hasFocus;

            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (_isOpen == false && SimulatorWindowProxy.isOpen)
            {
                onOpen?.Invoke();
                _isOpen = true;
            }

            if (_isOpen && SimulatorWindowProxy.isOpen == false)
            {
                onClose?.Invoke();
                _isOpen = false;
            }

            if ((_hasFocus == false) && SimulatorWindowProxy.hasFocus)
            {
                onFocus?.Invoke();
                _hasFocus = true;
            }

            if (_hasFocus && (SimulatorWindowProxy.hasFocus == false))
            {
                onLostFocus?.Invoke();
                _hasFocus = false;
            }
        }
    }
}
#endif