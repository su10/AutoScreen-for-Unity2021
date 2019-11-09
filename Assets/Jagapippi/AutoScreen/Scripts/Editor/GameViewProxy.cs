#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    public static class GameViewProxy
    {
        private const BindingFlags InstanceFlag = BindingFlags.Instance | BindingFlags.NonPublic;
        private const BindingFlags StaticFlag = BindingFlags.Static | BindingFlags.NonPublic;

        private static readonly Type GameView = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.GameView");
        private static readonly PropertyInfo HasFocus = GameView.GetProperty("hasFocus", InstanceFlag);
        private static readonly PropertyInfo CurrentSizeGroupType = GameView.GetProperty("currentSizeGroupType", StaticFlag);
        private static readonly PropertyInfo SelectedSizeIndex = GameView.GetProperty("selectedSizeIndex", InstanceFlag);
        private static readonly PropertyInfo CurrentGameViewSize = GameView.GetProperty("currentGameViewSize", InstanceFlag);

        internal static EditorWindow instance => EditorWindow.GetWindow(GameView, false, "Game", false);
        public static bool isOpen => (0 < Resources.FindObjectsOfTypeAll(GameView).Length);
        public static bool hasFocus => (isOpen && (bool) HasFocus.GetValue(instance, null));
        public static GameViewSizeGroupType currentSizeGroupType => (GameViewSizeGroupType) CurrentSizeGroupType.GetValue(instance, null);
        public static int selectedSizeIndex => (int) SelectedSizeIndex.GetValue(instance, null);

        public static GameViewSize currentGameViewSize
        {
            get
            {
                if (isOpen == false) return null;

                var gameViewSize = CurrentGameViewSize.GetValue(instance, null);
                var json = JsonUtility.ToJson(gameViewSize);
                return JsonUtility.FromJson<GameViewSize>(json);
            }
        }

        public static void Repaint() => instance.Repaint();
    }
}
#endif