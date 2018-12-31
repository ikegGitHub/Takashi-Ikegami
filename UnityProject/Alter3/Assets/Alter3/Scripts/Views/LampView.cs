using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, RequireComponent(typeof(Image))]
    public class LampView : MonoBehaviour
    {
        [SerializeField]
        private Color _onColor = Color.green;

        [SerializeField]
        private Color _offColor = Color.red;

        private Image _image;
        private bool _isOn;

        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;
                _image.color = _isOn ? _onColor : _offColor;
            }
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            IsOn = false;
        }
    }
}
