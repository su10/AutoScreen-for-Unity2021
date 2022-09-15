#if UNITY_EDITOR
using UnityEngine;
using UnityScreen = UnityEngine.Screen;

namespace Jagapippi.AutoScreen
{
    internal static class Screen
    {
        public static int width
        {
            get
            {
#if UNITY_EDITOR
                return ShimManagerProxy.width;
#else
                return UnityScreen.width;
#endif
            }
        }

        public static int height
        {
            get
            {
#if UNITY_EDITOR
                return ShimManagerProxy.height;
#else
                return UnityScreen.height;
#endif
            }
        }

        public static float dpi => UnityScreen.dpi;
        public static Resolution currentResolution => UnityScreen.currentResolution;
        public static Resolution[] resolutions => UnityScreen.resolutions;

        public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, int preferredRefreshRate = 0)
        {
            UnityScreen.SetResolution(width, height, fullscreenMode, preferredRefreshRate);
        }

        public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0)
        {
            UnityScreen.SetResolution(width, height, fullscreen, preferredRefreshRate);
        }

        public static bool fullScreen
        {
            get => UnityScreen.fullScreen;
            set => UnityScreen.fullScreen = value;
        }

        public static FullScreenMode fullScreenMode
        {
            get => UnityScreen.fullScreenMode;
            set => UnityScreen.fullScreenMode = value;
        }

        public static Rect safeArea => UnityScreen.safeArea;
        public static Rect[] cutouts => UnityScreen.cutouts;

        public static bool autorotateToPortrait
        {
            get => UnityScreen.autorotateToPortrait;
            set => UnityScreen.autorotateToPortrait = value;
        }

        public static bool autorotateToPortraitUpsideDown
        {
            get => UnityScreen.autorotateToPortraitUpsideDown;
            set => UnityScreen.autorotateToPortraitUpsideDown = value;
        }

        public static bool autorotateToLandscapeLeft
        {
            get => UnityScreen.autorotateToLandscapeLeft;
            set => UnityScreen.autorotateToLandscapeLeft = value;
        }

        public static bool autorotateToLandscapeRight
        {
            get => UnityScreen.autorotateToLandscapeRight;
            set => UnityScreen.autorotateToLandscapeRight = value;
        }

        public static ScreenOrientation orientation
        {
            get => UnityScreen.orientation;
            set => UnityScreen.orientation = value;
        }

        public static int sleepTimeout
        {
            get => UnityScreen.sleepTimeout;
            set => UnityScreen.sleepTimeout = value;
        }

        public static float brightness
        {
            get => UnityScreen.brightness;
            set => UnityScreen.brightness = value;
        }
    }
}
#endif
