#if UNITY_2019_2_OR_NEWER
using System.Linq;
#endif
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Jagapippi.AutoScreen.Demos
{
    [ExecuteAlways]
    public class ScreenSampler : MonoBehaviour
    {
        [SerializeField] private InputField _inputField = null;

        void Update()
        {
            var builder = new StringBuilder();

            builder.Append($"Screen.size: {Screen.size}\n");
            builder.Append($"Screen.safeArea:\n{Screen.safeArea}\n");
            builder.Append($"Screen.orientation: {Screen.orientation}\n");
#if UNITY_2019_2_OR_NEWER
            builder.Append($"Screen.brightness: {Screen.brightness}\n"); // 1 in editor
            builder.Append($"Screen.cutouts.Length: {Screen.cutouts.Length}\n");
            builder.Append($"Screen.cutouts: \n{string.Join(",\n", Screen.cutouts.Select(c => c.ToString()).ToList())}\n");
#endif

            _inputField.text = builder.ToString();
        }
    }
}