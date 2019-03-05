using System;

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

        public event Action<bool> OnEnableEyeCameraChanged = delegate { };
        public event Action<bool> OnEnableFaceCameraChanged = delegate { };
        public event Action<bool> OnEnableCollisionCheckChanged = delegate { };

        private bool _enableEyeCamera;
        private bool _enableFaceCamera;
        private bool _enableCollisionCheck;
    }
}
