using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, RequireComponent(typeof(Image))]
    public class LampView : MonoBehaviour
    {
        [SerializeField]
        private Sprite _offSprite = null;

        [SerializeField]
        private Sprite _onSprite = null;

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
                _image.sprite = _isOn ? _onSprite : _offSprite;
            }
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            IsOn = false;
        }
    }
}
