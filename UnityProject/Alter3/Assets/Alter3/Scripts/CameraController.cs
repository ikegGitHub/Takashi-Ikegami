using UnityEngine;
using UnityEngine.EventSystems;

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

        [SerializeField]
        private DragAreaView _dragAreView = null;

        private float _distance;
        private float _yAngle;
        private float _xAngle;

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
            _dragAreView.OnDrag += OnDrag;

            Reset();
        }

        private void OnDestroy()
        {
            _dragAreView.OnDrag -= OnDrag;
        }

        private void Update()
        {
            transform.localScale = new Vector3(1, 1, _distance);
            transform.localRotation = Quaternion.Euler(_xAngle, _yAngle, 0);

            _cameraTransform.LookAt(_lookAtTransform, Vector3.up);

            _distance = Mathf.Clamp(_distance + Input.mouseScrollDelta.y * 0.1f, _distanceMin, _distanceMax);
        }

        private void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.delta;
            _yAngle = Mathf.Repeat(_yAngle + delta.x * 0.1f, 360);
            _xAngle = Mathf.Repeat(_xAngle - delta.y * 0.1f, 360);
        }
    }
}
