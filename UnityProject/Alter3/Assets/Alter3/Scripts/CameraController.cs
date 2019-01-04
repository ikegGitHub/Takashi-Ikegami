using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _cameraTransform = null;

        [SerializeField]
        private Transform _lookAtTransform = null;

        [SerializeField]
        private float _distanceMin = 1.0f;

        [SerializeField]
        private float _distanceMax = 10.0f;

        private float _distance = 1;
        private float _yAngle = 0;
        private float _xAngle = 0;

        private bool _isMouseDown;
        private Vector3 _lastMousePosition;

        public void Reset()
        {
            _distance = 1.0f;
            _xAngle = 0;
            _yAngle = 0;

            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        private void Awake()
        {
            Reset();
        }

        private void Update()
        {
            transform.localScale = new Vector3(1, 1, _distance);
            transform.localRotation = Quaternion.Euler(_xAngle, _yAngle, 0);

            _cameraTransform.LookAt(_lookAtTransform, Vector3.up);

            if (_isMouseDown)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    _isMouseDown = false;
                }
                else
                {
                    var mousePosition = Input.mousePosition;
                    var delta = mousePosition - _lastMousePosition;
                    _yAngle = Mathf.Repeat(_yAngle + delta.x * 0.1f, 360);
                    _xAngle = Mathf.Repeat(_xAngle - delta.y * 0.1f, 360);
                    _lastMousePosition = mousePosition;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _lastMousePosition = Input.mousePosition;
                    _isMouseDown = true;
                }
            }

            _distance = Mathf.Clamp(_distance + Input.mouseScrollDelta.y * 0.1f, _distanceMin, _distanceMax);
        }
    }
}
