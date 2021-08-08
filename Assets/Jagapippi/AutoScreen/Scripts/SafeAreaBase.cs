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

        protected RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = this.GetComponent<RectTransform>();
                }

                return _rectTransform;
            }
        }

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
            SimulatorWindowEvent.onFocus += this.SetDirty;
            SimulatorWindowEvent.onLostFocus += this.SetDirty;
            Unity.DeviceSimulator.DeviceSimulatorCallbacks.OnDeviceChange += this.SetDirty;
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
            SimulatorWindowEvent.onFocus -= this.SetDirty;
            SimulatorWindowEvent.onLostFocus -= this.SetDirty;
            Unity.DeviceSimulator.DeviceSimulatorCallbacks.OnDeviceChange -= this.SetDirty;
            EditorSceneManager.sceneSaving -= this.OnSceneSaving;
            EditorSceneManager.sceneSaved -= this.OnSceneSaved;
            PrefabStage.prefabSaving -= this.OnPrefabSaving;
            PrefabStage.prefabSaved -= this.OnPrefabSaved;

            this.UnlockRect();
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

        void OnValidate() => this.SetDirty();

        void OnGUI()
        {
            if (_isDirty == false) return;

            _isDirty = false;
            this.TryUpdateRect();
        }
#endif
        void Start() => this.SetDirty();

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

            if (GUILayout.Button("Update Rect"))
            {
                safeArea.UpdateRect();
            }
        }
    }
#endif
}