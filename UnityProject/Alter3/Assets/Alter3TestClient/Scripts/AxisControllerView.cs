using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class AxisControllerView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _label = null;

        [SerializeField]
        private Slider _slider = null;

        [SerializeField]
        private TMP_InputField _valueInput = null;

        [SerializeField]
        private Text _jointNameText = null;

        [SerializeField]
        private Text _contentsText = null;

        public event Action<float> OnValueChanged = delegate { };

        public string LabelText
        {
            get
            {
                return _label.text;
            }
            set
            {
                _label.text = value;
            }
        }

        public string JointNameText
        {
            get
            {
                return _jointNameText.text;
            }
            set
            {
                _jointNameText.text = value;
            }
        }
        public string ContentsText
        {
            get
            {
                return _contentsText.text;
            }
            set
            {
                _contentsText.text = value;
            }
        }

        public float Value
        {
            get
            {
                return _slider.value;
            }
            set
            {
                _slider.value = value;
                ApplyValueToText();
            }
        }

        public float MinValue
        {
            get
            {
                return _slider.minValue;
            }
            set
            {
                _slider.minValue = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return _slider.maxValue;
            }
            set
            {
                _slider.maxValue = value;
            }
        }

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
            _valueInput.onEndEdit.AddListener(OnValueInputEndEdit);
            ApplyValueToText();
        }

        private void ApplyValueToText()
        {
            _valueInput.text = _slider.value.ToString("F02");
        }

        private void OnSliderValueChanged(float value)
        {
            ApplyValueToText();
            OnValueChanged(value);
        }

        private void OnValueInputEndEdit(string text)
        {
            Value = float.Parse(text);
        }
    }
}
