#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Jagapippi.AutoScreen
{
    public static class GameViewEvent
    {
        public static event Action opened;
        public static event Action<GameViewSize> resolutionChanged;
        public static event Action closed;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            var isOpen = false;

            EditorApplication.update += () =>
            {
                if (isOpen == false && GameViewProxy.isOpen)
                {
                    opened?.Invoke();
                    isOpen = true;
                }
            };

            var previousSizeIndex = -1;

            EditorApplication.update += () =>
            {
                if (GameViewProxy.hasFocus == false) return;

                var currentSizeIndex = GameViewProxy.selectedSizeIndex;
                if (previousSizeIndex == currentSizeIndex) return;

                resolutionChanged?.Invoke(GameViewProxy.currentGameViewSize);
                previousSizeIndex = currentSizeIndex;
            };

            EditorApplication.update += () =>
            {
                if (isOpen && GameViewProxy.isOpen == false)
                {
                    closed?.Invoke();
                    isOpen = false;
                }
            };
        }
    }
}
#endif