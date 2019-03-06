using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class DialView : MonoBehaviour, IDragHandler
    {
        [SerializeField]
        private RectTransform _dialRectTransform = null;

        [SerializeField]
        private Image _gaugeImage = null;

        private float _value;

        public event Action<float> OnValueChanged = delegate { };

        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = Mathf.Clamp01(value);
                UpdateUI();
                OnValueChanged(_value);
            }
        }

        private void Awake()
        {
            UpdateUI();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Value += eventData.delta.x * 0.001f;
        }

        private void UpdateUI()
        {
            var angles = _dialRectTransform.localEulerAngles;
            angles.z = Mathf.Lerp(0, -360, _value) - 90;
            _dialRectTransform.localEulerAngles = angles;
            _gaugeImage.fillAmount = _value;
        }
    }
}
