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

            UpdateLookAt();
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
            _distance = Mathf.Clamp(_distance + Input.mouseScrollDelta.y * 0.1f, _distanceMin, _distanceMax);
            transform.localScale = new Vector3(1, 1, _distance);
        }

        private void UpdateLookAt()
        {
            transform.localRotation = Quaternion.Euler(_xAngle, _yAngle, 0);

            _cameraTransform.LookAt(_lookAtTransform, Vector3.up);
        }

        private void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.delta;
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                float transSpeed = 0.2f;
                var delta2 = new Vector2(delta.x, -delta.y);
                transform.Translate(delta2 * Time.deltaTime * transSpeed);
                _lookAtTransform.Translate(delta2 * Time.deltaTime * transSpeed);

            }
            else
            {
                _yAngle = Mathf.Repeat(_yAngle + delta.x * 0.15f, 360);
                _xAngle = Mathf.Repeat(_xAngle + delta.y * 0.15f, 360);

                UpdateLookAt();

            }
        }
    }
}
