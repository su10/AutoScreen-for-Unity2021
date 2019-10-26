#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    public class CurrentGameViewScreen : ScriptableSingleton<CurrentGameViewScreen>
    {
        [SerializeField] private GameViewScreenAsset _asset;

        public static GameViewScreen value => instance._asset?.data;
        public static event Action<GameViewScreen> changed;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            GameViewEvent.resolutionChanged += size => instance.Load(size);
        }

        private void Load(GameViewSize size)
        {
            var previous = _asset;
            var current = GameViewScreenAsset.Load(size.baseText);

            if (previous == null && current == null) return;
            if (previous != null && previous.Equals(current)) return;

            _asset = current;
            changed?.Invoke(_asset?.data);
        }
    }
}
#endif