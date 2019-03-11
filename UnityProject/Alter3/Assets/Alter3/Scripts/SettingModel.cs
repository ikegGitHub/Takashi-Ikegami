using System;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class SettingModel
    {
        public bool EnableEyeCamera
        {
            get
            {
                return _enableEyeCamera;
            }
            set
            {
                if (_enableEyeCamera != value)
                {
                    _enableEyeCamera = value;
                    OnEnableEyeCameraChanged(_enableEyeCamera);
                }
            }
        }

        public bool EnableFaceCamera
        {
            get
            {
                return _enableFaceCamera;
            }
            set
            {
                if (_enableFaceCamera != value)
                {
                    _enableFaceCamera = value;
                    OnEnableFaceCameraChanged(_enableFaceCamera);
                }
            }
        }

        public bool EnableCollisionCheck
        {
            get
            {
                return _enableCollisionCheck;
            }
            set
            {
                if (_enableCollisionCheck != value)
                {
                    _enableCollisionCheck = value;
                    OnEnableCollisionCheckChanged(_enableCollisionCheck);
                }
            }
        }

        public bool EnableClothModel
        {
            get
            {
                return _enableClothModel;
            }
            set
            {
                if (_enableClothModel != value)
                {
                    _enableClothModel = value;
                    OnEnableClothModelChanged(_enableClothModel);
                }
            }
        }

        public bool CaptureScreen
        {
            get
            {
                return _captureScreen;
            }
            set
            {
                if (_captureScreen != value)
                {
                    _captureScreen = value;
                    OnCaptureScreenChanged(_captureScreen);
                }
            }
        }

        public int FrameRate
        {
            get
            {
                return Application.targetFrameRate;
            }
            set
            {
                Application.targetFrameRate = Mathf.Clamp(value, 1, 60);
            }
        }

        public event Action<bool> OnEnableEyeCameraChanged = delegate { };
        public event Action<bool> OnEnableFaceCameraChanged = delegate { };
        public event Action<bool> OnEnableCollisionCheckChanged = delegate { };
        public event Action<bool> OnEnableClothModelChanged = delegate { };
        public event Action<bool> OnCaptureScreenChanged = delegate { };
        public event Action<int> OnFrameRateChanged = delegate { };

        private bool _enableEyeCamera;
        private bool _enableFaceCamera;
        private bool _enableCollisionCheck;
        private bool _enableClothModel;
        private bool _captureScreen;
    }
}
