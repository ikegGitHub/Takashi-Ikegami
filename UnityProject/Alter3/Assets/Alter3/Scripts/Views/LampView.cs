using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, RequireComponent(typeof(Image))]
    public class LampView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Sprite _offSprite = null;

        [SerializeField]
        private Sprite _onSprite = null;

        [SerializeField]
        private Sprite _errorSprite = null;

        private Image _image;
        private bool _isOn;
        private bool _isError;

        public event Action<PointerEventData> OnClick = delegate { };

        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;
                UpdateSprite();
            }
        }

        public bool IsError
        {
            get
            {
                return _isError;
            }
            set
            {
                _isError = value;
                UpdateSprite();
            }
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            IsOn = false;
            IsError = false;
        }

        private void UpdateSprite()
        {
            if (_isError)
            {
                _image.sprite = _errorSprite;
            }
            else
            {
                _image.sprite = _isOn ? _onSprite : _offSprite;
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClick(eventData);
        }
    }
}
