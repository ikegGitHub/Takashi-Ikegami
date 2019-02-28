using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class AxisDialView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private DialView _dialView = null;

        [SerializeField]
        private TMP_Text _labelText = null;

        [SerializeField]
        private TMP_Text _valueText = null;

        private int _minValue;
        private int _maxValue;

        public event Action<float> OnValueChanged = delegate { };

        public event Action OnClicked = delegate { };

        public float Value
        {
            get
            {
                return Mathf.Lerp(_minValue, _maxValue, _dialView.Value);
            }
            set
            {
                _dialView.Value = (value - _minValue) / (_maxValue - _minValue);
            }
        }

        public void Initialize(int axisNumber, string label, int minValue, int maxValue, float value)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _labelText.text = $"{axisNumber}\n{label}";
            Value = value;
        }

        private void Awake()
        {
            _dialView.OnValueChanged += value =>
            {
                _valueText.text = Value.ToString("F01");
                OnValueChanged(Value);
            };
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClicked();
        }
    }
}
