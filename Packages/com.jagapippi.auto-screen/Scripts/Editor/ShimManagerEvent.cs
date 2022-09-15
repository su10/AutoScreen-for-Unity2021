#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityScreen = UnityEngine.Screen;

namespace Jagapippi.AutoScreen
{
    internal static class ShimManagerEvent
    {
        public static event Action onActiveShimChanged;

        private static object _activeScreenShim;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            _activeScreenShim = ShimManagerProxy.GetActiveScreenShim();

            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            var currentActiveScreenShim = ShimManagerProxy.GetActiveScreenShim();

            if (_activeScreenShim != currentActiveScreenShim)
            {
                _activeScreenShim = currentActiveScreenShim;
                onActiveShimChanged?.Invoke();
            }
        }
    }
}
#endif
