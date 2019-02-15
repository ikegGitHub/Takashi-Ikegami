using UnityEngine;
using UnityEngine.Assertions;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class FaceCameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetTransform = null;

        [SerializeField]
        private float _distance = 1;

        [SerializeField]
        private float _offsetHeight = 0;

        private void Awake()
        {
            Assert.IsNotNull(_targetTransform);
        }

        private void Update()
        {
            if (_targetTransform != null)
            {
                transform.position = _targetTransform.position + _targetTransform.forward * _distance;
                transform.LookAt(_targetTransform, _targetTransform.up);
                transform.position += _targetTransform.up * _offsetHeight;
            }
        }
    }
}
