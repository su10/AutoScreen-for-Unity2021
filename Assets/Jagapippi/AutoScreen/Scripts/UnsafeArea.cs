using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Jagapippi.AutoScreen
{
    [ExecuteAlways]
    public class UnsafeArea : MonoBehaviour
    {
        public enum Position
        {
            Top,
            Bottom,
            Left,
            Right,
        }

        [SerializeField] private Position _position;

        public Position position
        {
            get { return _position; }
            set { _position = value; }
        }

        private RectTransform _rectTransform;
        private RectTransform rectTransform => (_rectTransform != null) ? _rectTransform : (_rectTransform = this.GetComponent<RectTransform>());

        void Reset()
        {
            this.ResetRect();
            this.UpdateRect();
        }

        private void ResetRect()
        {
            this.rectTransform.sizeDelta = this.rectTransform.anchoredPosition = Vector3.zero;
            this.rectTransform.anchorMin = Vector2.zero;
            this.rectTransform.anchorMax = Vector2.zero;
            this.rectTransform.localRotation = Quaternion.identity;
            this.rectTransform.localScale = Vector3.one;
        }

#if UNITY_EDITOR
        void OnEnable()
        {
            GameViewEvent.resolutionChanged += gameViewSize =>
            {
                if (this != null) this.UpdateRect();
            };
            EditorSceneManager.sceneSaving += (scene, path) => this.ResetRect();
            EditorSceneManager.sceneSaved += scene => this.UpdateRect();

            this.rectTransform.hideFlags = HideFlags.NotEditable;
        }

        void OnDisable() => this.rectTransform.hideFlags = HideFlags.None;

        private bool _isDirty = false;

        void OnValidate() => _isDirty = true;

        void Update()
        {
            if (_isDirty)
            {
                _isDirty = false;
                this.UpdateRect();
            }
        }
#endif
        void Start() => this.UpdateRect();

        public void UpdateRect() => this.UpdateRect(Screen.safeArea, Screen.width, Screen.height);

        public void UpdateRect(Rect safeArea, int width, int height)
        {
            if ((safeArea.width == width) && (safeArea.height == height))
            {
                this.ResetRect();
                return;
            }

            var anchorMin = Vector2.zero;
            var anchorMax = Vector2.one;

            switch (this.position)
            {
                case Position.Top:
                    anchorMin = new Vector2(0, safeArea.height + safeArea.y) / height;
                    break;
                case Position.Bottom:
                    anchorMax = new Vector2(1, safeArea.y / height);
                    break;
                case Position.Left:
                    anchorMax = new Vector2(safeArea.x / width, 1);
                    break;
                case Position.Right:
                    anchorMin = new Vector2(safeArea.width + safeArea.x, 0) / width;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.rectTransform.anchorMin = anchorMin;
            this.rectTransform.anchorMax = anchorMax;
            this.rectTransform.anchoredPosition = Vector3.zero;
            this.rectTransform.sizeDelta = Vector2.zero;
        }
    }
}