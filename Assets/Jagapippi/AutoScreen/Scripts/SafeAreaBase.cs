using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Jagapippi.AutoScreen
{
    [ExecuteAlways]
    public abstract class SafeAreaBase : MonoBehaviour
    {
        private RectTransform _rectTransform;
        protected RectTransform rectTransform => _rectTransform ?? (_rectTransform = this.GetComponent<RectTransform>());

        void Reset()
        {
            this.ResetRect();
            this.UpdateRect();
        }

        protected virtual void ResetRect()
        {
            this.rectTransform.sizeDelta = Vector3.zero;
            this.rectTransform.anchoredPosition = Vector3.zero;
            this.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            this.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            this.rectTransform.localRotation = Quaternion.identity;
            this.rectTransform.localScale = Vector3.one;
        }

#if UNITY_EDITOR
        void OnEnable()
        {
            GameViewEvent.resolutionChanged += gameViewSize =>
            {
                if (this != null && this.enabled) this.UpdateRect();
            };
            EditorSceneManager.sceneSaving += (scene, path) =>
            {
                if (this.rectTransform) this.ResetRect();
            };
            EditorSceneManager.sceneSaved += scene =>
            {
                if (this.rectTransform) this.UpdateRect();
            };

            this.rectTransform.hideFlags = HideFlags.NotEditable;
        }

        void OnDisable() => this.rectTransform.hideFlags = HideFlags.None;

        private bool _isDirty;

        void OnValidate() => _isDirty = true;

        void Update()
        {
            if (_isDirty == false) return;

            _isDirty = false;
            this.UpdateRect();
        }
#endif
        void Start() => this.UpdateRect();

        public void UpdateRect() => this.UpdateRect(Screen.safeArea, Screen.width, Screen.height);
        public abstract void UpdateRect(Rect safeArea, int width, int height);
    }
}