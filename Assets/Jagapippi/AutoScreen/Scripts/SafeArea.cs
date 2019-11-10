using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Jagapippi.AutoScreen
{
    [ExecuteAlways]
    public class SafeArea : MonoBehaviour
    {
        [Flags]
        public enum Padding
        {
            Top = 1 << 0,
            Bottom = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
        }

        [SerializeField, EnumFlags] private Padding _padding = (Padding) Enum.Parse(typeof(Padding), (-1).ToString());

        public Padding padding
        {
            get { return _padding; }
            set { _padding = value; }
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
            this.rectTransform.anchorMax = Vector2.one;
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

            var paddingTop = 0f;
            var paddingRight = 0f;
            var paddingLeft = 0f;
            var paddingBottom = 0f;

            if (this.padding.HasFlag(Padding.Top)) paddingTop = height - (safeArea.height + safeArea.y);
            if (this.padding.HasFlag(Padding.Right)) paddingRight = width - (safeArea.width + safeArea.x);
            if (this.padding.HasFlag(Padding.Bottom)) paddingBottom = safeArea.y;
            if (this.padding.HasFlag(Padding.Left)) paddingLeft = safeArea.x;

            this.rectTransform.sizeDelta = this.rectTransform.anchoredPosition = Vector3.zero;
            this.rectTransform.anchorMin = new Vector2(paddingLeft / width, paddingBottom / height);
            this.rectTransform.anchorMax = new Vector2((width - paddingRight) / width, (height - paddingTop) / height);
        }
    }
}