using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button _button = null;

        [SerializeField]
        private TMP_Text _buttonText = null;

        public event Action OnClick;

        public string ButtonText
        {
            get
            {
                return _buttonText.text;
            }
            set
            {
                _buttonText.text = value;
            }
        }

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClick?.Invoke());
        }
    }
}
