using UnityEngine;
using UnityScreen = UnityEngine.Screen;

namespace Jagapippi.AutoScreen
{
    public static class Screen
    {
        public static int width
        {
            get
            {
#if UNITY_EDITOR
                return GameViewProxy.currentGameViewSize?.width ?? UnityScreen.width;
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
                return GameViewProxy.currentGameViewSize?.height ?? UnityScreen.height;
#else
                return UnityScreen.height;
#endif
            }
        }

        public static Vector2 size => new Vector2(width, height);

        public static Rect safeArea
        {
            get
            {
#if UNITY_EDITOR
                return CurrentGameViewScreen.value?.safeArea ?? new Rect(Vector2.zero, size);
#else
                return UnityScreen.safeArea;
#endif
            }
        }

        public static ScreenOrientation orientation
        {
            get { return UnityScreen.orientation; }
            set { UnityScreen.orientation = value; }
        }

        public static bool autorotateToLandscapeLeft
        {
            get { return UnityScreen.autorotateToLandscapeLeft; }
            set { UnityScreen.autorotateToLandscapeLeft = value; }
        }

        public static bool autorotateToLandscapeRight
        {
            get { return UnityScreen.autorotateToLandscapeRight; }
            set { UnityScreen.autorotateToLandscapeRight = value; }
        }

        public static bool autorotateToPortrait
        {
            get { return UnityScreen.autorotateToPortrait; }
            set { UnityScreen.autorotateToPortrait = value; }
        }

        public static bool autorotateToPortraitUpsideDown
        {
            get { return UnityScreen.autorotateToPortraitUpsideDown; }
            set { UnityScreen.autorotateToPortraitUpsideDown = value; }
        }

        public static Resolution currentResolution => UnityScreen.currentResolution;
        public static float dpi => UnityScreen.dpi;

        public static bool fullScreen
        {
            get { return UnityScreen.fullScreen; }
            set { UnityScreen.fullScreen = value; }
        }

        public static FullScreenMode fullScreenMode
        {
            get { return UnityScreen.fullScreenMode; }
            set { UnityScreen.fullScreenMode = value; }
        }

        public static Resolution[] resolutions => UnityScreen.resolutions;

        public static int sleepTimeout
        {
            get { return UnityScreen.sleepTimeout; }
            set { UnityScreen.sleepTimeout = value; }
        }

        public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0)
        {
            UnityScreen.SetResolution(width, height, fullscreen, preferredRefreshRate);
        }

        public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, int preferredRefreshRate = 0)
        {
            UnityScreen.SetResolution(width, height, fullscreenMode, preferredRefreshRate);
        }

#if UNITY_2019_2_OR_NEWER
        public static float brightness => UnityScreen.brightness;
        public static Rect[] cutouts => UnityScreen.cutouts;
#endif
    }
}