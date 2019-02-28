using UnityEngine;
using UnityEngine.EventSystems;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        private const float Agility = 0.5f;
        private const float NormalSpeed = 0.2f;
        private const float SlowSpeed = 0.05f;

        [SerializeField]
        private DragAreaView _dragAreaView = null;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        private bool IsShiftKeyDown => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        private float Speed => IsShiftKeyDown ? SlowSpeed : NormalSpeed;

        public Transform LookTarget { get; set; }

        public void ResetPosition()
        {
            _targetPosition = _initialPosition;
            _targetRotation = _initialRotation;
        }

        public void MoveToForwardOfTarget()
        {
            if (LookTarget == null)
            {
                Debug.LogError("LookTarget is not set");
                return;
            }

            _targetPosition = LookTarget.position + 3 * LookTarget.forward + 2 * LookTarget.up + LookTarget.right;
            _targetRotation = Quaternion.LookRotation(LookTarget.position + 0.5f * LookTarget.up - _targetPosition, LookTarget.up);
        }

        private void Awake()
        {
            _dragAreaView.OnDrag += OnDrag;
            _dragAreaView.OnScroll += OnScroll;

            _initialPosition = transform.localPosition;
            _initialRotation = transform.localRotation;
            _targetPosition = _initialPosition;
            _targetRotation = _initialRotation;
        }

        private void OnDestroy()
        {
            _dragAreaView.OnDrag -= OnDrag;
            _dragAreaView.OnScroll -= OnScroll;
        }

        private void Update()
        {
            transform.localPosition += Agility * (_targetPosition - transform.localPosition);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, Agility);
        }

        private void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.delta;

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                case PointerEventData.InputButton.Middle:
                    var delta2 = -transform.right * delta.x - transform.up * delta.y;
                    _targetPosition += delta2 * Time.deltaTime * Speed;
                    break;
                case PointerEventData.InputButton.Right:
                    var angles = _targetRotation.eulerAngles;
                    angles.y += delta.x * Speed;
                    angles.x += -delta.y * Speed;
                    _targetRotation.eulerAngles = angles;
                    break;
            }
        }

        private void OnScroll(PointerEventData eventData)
        {
            _targetPosition += transform.forward * eventData.scrollDelta.y * Speed;
        }
    }
}
