using System.Linq;
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

            builder.Append($"Screen.size: {new Vector2(Screen.width, Screen.height)}\n");
            builder.Append($"Screen.safeArea:\n{Screen.safeArea}\n");
            builder.Append($"Screen.orientation: {Screen.orientation}\n");
            builder.Append($"Screen.brightness: {Screen.brightness}\n"); // 1 in editor
            builder.Append($"Screen.cutouts.Length: {Screen.cutouts.Length}\n");
            builder.Append($"Screen.cutouts: \n{string.Join(",\n", Screen.cutouts.Select(c => c.ToString()).ToList())}\n");

            _inputField.text = builder.ToString();
        }
    }
}