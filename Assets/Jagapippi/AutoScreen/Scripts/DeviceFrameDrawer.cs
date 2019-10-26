using UnityEngine;

namespace Jagapippi.AutoScreen
{
    [ExecuteAlways]
    public class DeviceFrameDrawer : MonoBehaviour
    {
#if UNITY_EDITOR
        private Sprite _frame;

        void OnEnable()
        {
            CurrentGameViewScreen.changed += screen => _frame = screen?.frame;
            _frame = CurrentGameViewScreen.value?.frame;
        }

        void OnGUI()
        {
            if (_frame == null) return;

            GUI.DrawTexture(
                position: _frame.textureRect,
                image: _frame.texture,
                scaleMode: ScaleMode.StretchToFill,
                alphaBlend: true,
                imageAspect: 0,
                color: Color.black,
                borderWidth: 0,
                borderRadius: 0
            );
        }
#endif
    }
}