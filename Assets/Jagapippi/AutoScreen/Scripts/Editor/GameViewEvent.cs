#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Jagapippi.AutoScreen
{
    public static class GameViewEvent
    {
        public static event Action<GameViewSize> resolutionChanged;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            var previousSizeIndex = -1;

            EditorApplication.update += () =>
            {
                if (GameViewProxy.hasFocus == false) return;

                var currentSizeIndex = GameViewProxy.selectedSizeIndex;
                if (previousSizeIndex == currentSizeIndex) return;

                resolutionChanged?.Invoke(GameViewProxy.currentGameViewSize);
                previousSizeIndex = currentSizeIndex;
            };
        }
    }
}
#endif