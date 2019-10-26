using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Jagapippi.AutoScreen
{
    [ExecuteAlways]
    public class SafeAreaDrawer : MonoBehaviour
    {
        [SerializeField] private float _borderWidth = 2;
        [SerializeField] private Color _borderColor = Color.green;

        private Texture2D _texture;
        private Texture2D texture => (_texture != null) ? _texture : (_texture = new Texture2D(1, 1));
        private ScreenOrientation _orientation;
        private Rect _rect;

        void OnEnable()
        {
            _orientation = Screen.orientation;

#if UNITY_EDITOR
            CurrentGameViewScreen.changed += OnScreenChanged;
            this.OnScreenChanged(CurrentGameViewScreen.value);
#else
            this.OnScreenChanged();
#endif
        }

        void Update()
        {
            if (_orientation != Screen.orientation)
            {
                this.OnScreenChanged();
                _orientation = Screen.orientation;
            }
        }

        void OnGUI()
        {
            if (_rect == Rect.zero) return;

            GUI.DrawTexture(
                position: _rect,
                image: this.texture,
                scaleMode: ScaleMode.StretchToFill,
                alphaBlend: true,
                imageAspect: 0,
                color: _borderColor,
                borderWidth: _borderWidth,
                borderRadius: 0
            );
        }

#if UNITY_EDITOR
        private void OnScreenChanged(GameViewScreen screen)
        {
            if (screen != null)
            {
                this.OnScreenChanged(screen.safeArea, screen.size);
            }
            else
            {
                _rect = Rect.zero;
            }
        }
#endif

        private void OnScreenChanged(Rect safeArea, Vector2 screenSize)
        {
            if (safeArea.size != screenSize)
            {
                // NOTE: Screen.safeArea's origin is left-bottom, but GUI's origin is left-top.
                _rect = InvertY(safeArea, screenSize.y);
            }
            else
            {
                _rect = Rect.zero;
            }
        }

        private void OnScreenChanged()
        {
            this.OnScreenChanged(Screen.safeArea, Screen.size);
        }

        private static Rect InvertY(Rect rect, float totalHeight)
        {
            var paddingTop = totalHeight - (rect.height + rect.y);
            return new Rect(rect.x, paddingTop, rect.width, rect.height);
        }
    }
}