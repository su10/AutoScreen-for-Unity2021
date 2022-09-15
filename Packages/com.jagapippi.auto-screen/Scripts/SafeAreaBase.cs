using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
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
        protected RectTransform rectTransform => (_rectTransform != null) ? _rectTransform : _rectTransform = this.GetComponent<RectTransform>();

        void Reset()
        {
            this.ResetRect();
            this.UpdateRect();

            if (this.GetComponent<RuntimeSafeAreaUpdater>() == false)
            {
                this.gameObject.AddComponent<RuntimeSafeAreaUpdater>();
            }
        }

        public virtual void ResetRect()
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
        private void LockRect() => this.rectTransform.hideFlags = HideFlags.NotEditable;
        private void UnlockRect() => this.rectTransform.hideFlags = HideFlags.None;

        void OnEnable()
        {
            SimulatorWindowEvent.onOpen += this.SetDirty;
            SimulatorWindowEvent.onClose += this.SetDirty;
            SimulatorWindowEvent.onOrientationChanged += this.OnOrientationChanged;
            ShimManagerEvent.onActiveShimChanged += this.SetDirty;
            EditorSceneManager.sceneSaving += this.OnSceneSaving;
            EditorSceneManager.sceneSaved += this.OnSceneSaved;
            PrefabStage.prefabSaving += this.OnPrefabSaving;
            PrefabStage.prefabSaved += this.OnPrefabSaved;

            this.LockRect();
            this.TryUpdateRect();
        }

        void OnDisable()
        {
            SimulatorWindowEvent.onOpen -= this.SetDirty;
            SimulatorWindowEvent.onClose -= this.SetDirty;
            SimulatorWindowEvent.onOrientationChanged -= this.OnOrientationChanged;
            ShimManagerEvent.onActiveShimChanged -= this.SetDirty;
            EditorSceneManager.sceneSaving -= this.OnSceneSaving;
            EditorSceneManager.sceneSaved -= this.OnSceneSaved;
            PrefabStage.prefabSaving -= this.OnPrefabSaving;
            PrefabStage.prefabSaved -= this.OnPrefabSaved;

            this.UnlockRect();
        }

        private void OnOrientationChanged(ScreenOrientation orientation)
        {
            if (EditorApplication.isPlaying == false)
            {
                this.SetDirty();
            }
        }

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

        private void SetDirty() => _isDirty = true;

        void OnValidate()
        {
            if (EditorApplication.isPlaying == false)
            {
                this.SetDirty();
            }
        }

        void OnGUI()
        {
            EditorApplication.delayCall += () =>
            {
                if (_isDirty == false) return;

                _isDirty = false;
                this.TryUpdateRect();

                if (EditorApplication.isPlaying == false)
                {
#if UNITY_2021_1
                    SimulatorWindowProxy.Repaint();
#else
                    SimulatorWindowProxy.RepaintWithDelay();
#endif
                }
            };
        }
#endif
        void Start()
        {
#if UNITY_EDITOR
            this.SetDirty();
#else
            this.UpdateRect();
#endif
        }

        public void UpdateRect() => this.UpdateRect(Screen.safeArea, Screen.width, Screen.height);
        public abstract void UpdateRect(Rect safeArea, int width, int height);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SafeAreaBase), true)]
    public class SafeAreaBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var safeArea = this.target as SafeAreaBase;

            using (new EditorGUI.DisabledScope(Application.isPlaying == false))
            {
                if (GUILayout.Button("Update Rect"))
                {
                    safeArea.UpdateRect();
                    SimulatorWindowProxy.Repaint();
                }
            }
        }
    }
#endif
}
