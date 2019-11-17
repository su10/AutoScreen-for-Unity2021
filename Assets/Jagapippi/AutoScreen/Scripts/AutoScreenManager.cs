using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Jagapippi.AutoScreen
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public sealed class AutoScreenManager : MonoBehaviour
    {
#if UNITY_EDITOR

        #region static

        private static readonly string prefabSearchQuery = $"l:{AutoScreenSettings.assetLabel} t:Prefab";

        private static AutoScreenManager _Instance;
        private static AutoScreenManager Instance => (_Instance != null) ? _Instance : (_Instance = FindOrInstantiate());

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            CurrentGameViewScreen.changed += screen => _Instance = FindOrInstantiate();
            EditorSceneManager.sceneClosed += _ => _Instance = FindOrInstantiate();
            EditorApplication.playModeStateChanged += mode => _Instance = FindOrInstantiate();
        }

        private static AutoScreenManager FindOrInstantiate()
        {
            var instance = FindInstanceInHierarchy();
            if (instance == null) instance = Instantiate();
            return instance;
        }

        private static AutoScreenManager FindInstanceInHierarchy()
        {
            return Resources.FindObjectsOfTypeAll<AutoScreenManager>().FirstOrDefault(manager => manager.IsInHierarchy());
        }

        private static AutoScreenManager Instantiate()
        {
            return (PrefabUtility.InstantiatePrefab(Prefab.gameObject) as GameObject).GetComponent<AutoScreenManager>();
        }

        private static AutoScreenManager _Prefab;

        private static AutoScreenManager Prefab
        {
            get
            {
                if (_Prefab == null)
                {
                    _Prefab = AssetDatabase.FindAssets(prefabSearchQuery)
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Select(AssetDatabase.LoadAssetAtPath<AutoScreenManager>)
                        .First(prefab => prefab != null);
                }

                return _Prefab;
            }
        }

        #endregion

        [SerializeField] private HideFlags _hideFlags = HideFlags.HideAndDontSave;

        void Start()
        {
            if (this.IsInHierarchy() == false) return;

            foreach (var instance in Resources.FindObjectsOfTypeAll<AutoScreenManager>())
            {
                if (instance.IsInPrefabAsset()) continue;
                if (instance.IsInPrefabStage()) continue;
                if (instance == this) continue;

                if (EditorApplication.isPlaying)
                {
                    Destroy(instance.gameObject);
                }
                else
                {
                    DestroyImmediate(instance.gameObject);
                }
            }
        }

        void Update()
        {
            if (this.IsInPrefabAsset() || this.IsInPrefabStage()) return;

            if (this.gameObject.hideFlags != _hideFlags)
            {
                this.gameObject.hideFlags = _hideFlags;
            }
        }
#endif
    }
}