using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ConnectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text = null;

        [SerializeField]
        private Button _disconnectButton = null;

        public event Action OnDisconnectButtonClicked = delegate { };

        public string Text
        {
            get
            {
                return _text.text;
            }
            set
            {
                _text.text = value;
            }
        }

        private void Awake()
        {
            _disconnectButton.onClick.AddListener(() => OnDisconnectButtonClicked());
        }
    }
}
