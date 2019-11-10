using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Jagapippi.AutoScreen
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameViewScreen")]
    public sealed class GameViewScreenAsset : ScriptableObject
    {
#if UNITY_EDITOR
        public static GameViewScreenAsset Load(string baseText)
        {
            return AssetDatabase.FindAssets($"t:{nameof(GameViewScreenAsset)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameViewScreenAsset>)
                .FirstOrDefault(asset => asset.data.baseText == baseText);
        }
#endif

        [SerializeField] private GameViewScreen _data = new GameViewScreen();
        public GameViewScreen data => _data;
    }
}