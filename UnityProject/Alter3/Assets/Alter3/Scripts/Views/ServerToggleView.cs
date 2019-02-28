using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ServerToggleView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Animator _animator = null;

        public event Action<bool> OnValueChanged = delegate { };

        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;
                _animator.Play(_isOn ? "On" : "Off", 0, 0);
                OnValueChanged(_isOn);
            }
        }

        private bool _isOn;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            IsOn = !IsOn;
        }
    }
}
