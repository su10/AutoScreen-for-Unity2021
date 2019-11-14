#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    public static class PrefabHelper
    {
        public static bool IsInHierarchy(Component component)
        {
            return component.gameObject.scene.IsValid();
        }

        public static bool IsInPrefabAsset(Component component)
        {
            return AssetDatabase.Contains(component.transform.root.gameObject);
        }

        public static bool IsInPrefabStage(Component component)
        {
            return PrefabStageUtility.GetCurrentPrefabStage() != null && IsInHierarchy(component);
        }
    }
}
#endif