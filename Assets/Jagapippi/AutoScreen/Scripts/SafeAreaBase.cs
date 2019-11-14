using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
#endif

namespace Jagapippi.AutoScreen
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public abstract class SafeAreaBase : MonoBehaviour, ISafeAreaUpdatable
    {
        private RectTransform _rectTransform;
        protected RectTransform rectTransform => _rectTransform ?? (_rectTransform = this.GetComponent<RectTransform>());

        void Reset()
        {
            this.ResetRect();
            this.UpdateRect();

            if (this.GetComponent<RuntimeSafeAreaUpdater>() == false)
            {
                this.gameObject.AddComponent<RuntimeSafeAreaUpdater>();
            }
        }

        protected virtual void ResetRect()
        {
            this.rectTransform.sizeDelta = Vector3.zero;
            this.rectTransform.anchoredPosition = Vector3.zero;
            this.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            this.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            this.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            this.rectTransform.localRotation = Quaternion.identity;
            this.rectTransform.localScale = Vector3.one;
        }

#if UNITY_EDITOR
        void OnEnable()
        {
            GameViewEvent.resolutionChanged += this.OnResolutionChanged;
            EditorSceneManager.sceneSaving += this.OnSceneSaving;
            EditorSceneManager.sceneSaved += this.OnSceneSaved;
            PrefabStage.prefabSaving += this.OnPrefabSaving;
            PrefabStage.prefabSaved += this.OnPrefabSaved;

            this.rectTransform.hideFlags = HideFlags.NotEditable;
        }

        void OnDisable()
        {
            GameViewEvent.resolutionChanged -= this.OnResolutionChanged;
            EditorSceneManager.sceneSaving -= this.OnSceneSaving;
            EditorSceneManager.sceneSaved -= this.OnSceneSaved;
            PrefabStage.prefabSaving -= this.OnPrefabSaving;
            PrefabStage.prefabSaved -= this.OnPrefabSaved;

            this.rectTransform.hideFlags = HideFlags.None;
        }

        private void OnResolutionChanged(GameViewSize size) => this.TryUpdateRect();
        private void OnSceneSaving(Scene scene, string path) => this.TryResetRect();
        private void OnSceneSaved(Scene scene) => this.TryUpdateRect();
        private void OnPrefabSaving(GameObject prefabContentsRoot) => this.TryResetRect();
        private void OnPrefabSaved(GameObject prefabContentsRoot) => this.TryUpdateRect();

        private void TryResetRect()
        {
            if (this.rectTransform) this.ResetRect();
        }

        private void TryUpdateRect()
        {
            if (this.rectTransform) this.UpdateRect();
        }

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