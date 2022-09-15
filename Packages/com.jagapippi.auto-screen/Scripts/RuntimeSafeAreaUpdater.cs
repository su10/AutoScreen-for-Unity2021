using UnityEngine;

namespace Jagapippi.AutoScreen
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ISafeAreaUpdatable))]
    public class RuntimeSafeAreaUpdater : MonoBehaviour
    {
        private ISafeAreaUpdatable _target;
        private Rect _safeArea;

        void Start()
        {
            _target = this.GetComponent<ISafeAreaUpdatable>();

            _safeArea = Screen.safeArea;
            _target.UpdateRect();
        }

        void Update()
        {
            if (_safeArea == Screen.safeArea) return;

            _safeArea = Screen.safeArea;
            _target.UpdateRect();
        }
    }
}
