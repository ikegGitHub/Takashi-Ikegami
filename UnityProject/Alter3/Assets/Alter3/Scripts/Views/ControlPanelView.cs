using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ControlPanelView : MonoBehaviour
    {
        [SerializeField]
        private Toggle _enableEyeCameraToggle = null;

        [SerializeField]
        private Toggle _enableFaceCameraToggle = null;

        [SerializeField]
        private Toggle _enableCollisionCheckToggle = null;

        [SerializeField]
        private Toggle _enableClothModelToggle = null;

        [SerializeField]
        private Toggle _captureScreenToggle = null;

        [SerializeField]
        private Button _resetCameraButton = null;

        [SerializeField]
        private Button _resetPositionButton = null;

        public event Action<bool> OnEnableEyeCameraChanged = delegate { };
        public event Action<bool> OnEnableFaceCameraChanged = delegate { };
        public event Action<bool> OnEnableCollisionCheckChanged = delegate { };
        public event Action<bool> OnEnableClothModelChanged = delegate { };
        public event Action<bool> OnCaptureScreenChanged = delegate { };
        public event Action OnResetCameraButtonClicked = delegate { };
        public event Action OnResetPositionButtonClicked = delegate { };

        public bool EnableEyeCamera
        {
            get
            {
                return _enableEyeCameraToggle.isOn;
            }
            set
            {
                _enableEyeCameraToggle.isOn = value;
            }
        }

        public bool EnableFaceCamera
        {
            get
            {
                return _enableFaceCameraToggle.isOn;
            }
            set
            {
                _enableFaceCameraToggle.isOn = value;
            }
        }

        public bool EnableCollisionCheck
        {
            get
            {
                return _enableCollisionCheckToggle.isOn;
            }
            set
            {
                _enableCollisionCheckToggle.isOn = value;
            }
        }

        public bool EnableClothModel
        {
            get
            {
                return _enableClothModelToggle.isOn;
            }
            set
            {
                _enableClothModelToggle.isOn = value;
            }
        }

        public bool CaptureScreen
        {
            get
            {
                return _captureScreenToggle.isOn;
            }
            set
            {
                _captureScreenToggle.isOn = value;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(_enableEyeCameraToggle);
            Assert.IsNotNull(_enableFaceCameraToggle);
            Assert.IsNotNull(_enableCollisionCheckToggle);
            Assert.IsNotNull(_enableClothModelToggle);
            Assert.IsNotNull(_captureScreenToggle);
            Assert.IsNotNull(_resetCameraButton);
            Assert.IsNotNull(_resetPositionButton);

            _enableEyeCameraToggle.onValueChanged.AddListener(isOn => OnEnableEyeCameraChanged(isOn));
            _enableFaceCameraToggle.onValueChanged.AddListener(isOn => OnEnableFaceCameraChanged(isOn));
            _enableCollisionCheckToggle.onValueChanged.AddListener(isOn => OnEnableCollisionCheckChanged(isOn));
            _enableClothModelToggle.onValueChanged.AddListener(isOn => OnEnableClothModelChanged(isOn));
            _captureScreenToggle.onValueChanged.AddListener(isOn => OnCaptureScreenChanged(isOn));
            _resetCameraButton.onClick.AddListener(() => OnResetCameraButtonClicked());
            _resetPositionButton.onClick.AddListener(() => OnResetPositionButtonClicked());
        }
    }
}
