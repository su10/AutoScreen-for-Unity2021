using UnityEngine;

namespace Jagapippi.AutoScreen
{
    [DisallowMultipleComponent]
    public class RuntimeSafeAreaUpdater : MonoBehaviour
    {
        private ISafeAreaUpdatable _target;
        private ScreenOrientation _orientation;

        void Start()
        {
            _target = this.GetComponent<ISafeAreaUpdatable>();

            _orientation = Screen.orientation;
            _target.UpdateRect();
        }

        void Update()
        {
            if (_orientation == Screen.orientation) return;

            _orientation = Screen.orientation;
            _target.UpdateRect();
        }
    }
}