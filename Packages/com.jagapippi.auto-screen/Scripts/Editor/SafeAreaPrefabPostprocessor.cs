#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Jagapippi.AutoScreen
{
    class SafeAreaPrefabPostprocessor : AssetPostprocessor
    {
        private static readonly List<string> _processedPathList = new List<string>();

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                if (Path.GetExtension(path) != ".prefab") continue;
                if (_processedPathList.Remove(path)) continue;

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                foreach (var safeArea in prefab.GetComponentsInChildren<ISafeAreaUpdatable>(true))
                {
                    safeArea.ResetRect();
                    _processedPathList.Add(path);
                }
            }
        }
    }
}
#endif
