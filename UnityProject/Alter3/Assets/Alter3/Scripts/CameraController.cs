﻿using UnityEngine;
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

        [SerializeField]
        private Transform _target = null;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        private bool IsShiftKeyDown => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        private float Speed => IsShiftKeyDown ? SlowSpeed : NormalSpeed;

        public void ResetPosition()
        {
            _targetPosition = _initialPosition;
            _targetRotation = _initialRotation;
        }

        public void MoveToForwardOfTarget()
        {
            _targetPosition = _target.position + 3 * _target.forward + 2 * _target.up + _target.right;
            _targetRotation = Quaternion.LookRotation(_target.position + 0.5f * _target.up - _targetPosition, _target.up);
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
