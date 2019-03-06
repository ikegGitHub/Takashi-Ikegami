using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class CaptureCameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int _resolution = new Vector2Int(256, 256);

        [SerializeField]
        private int _frameInterval = 3;

        private bool _enabled;
        private Camera _camera;
        private RenderTexture _buffer;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                _camera.enabled = _enabled;
                enabled = _enabled;
            }
        }

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            Assert.IsNotNull(_camera);
            Assert.IsTrue(_frameInterval >= 1);

            _buffer = new RenderTexture(_resolution.x, _resolution.y, 24, RenderTextureFormat.ARGB32);
            _buffer.Create();
            _camera.targetTexture = _buffer;
        }

        private void Update()
        {
            if (Time.frameCount % _frameInterval == 0)
            {
                CaptureAndSave();
            }
        }

        private void CaptureAndSave()
        {
            SaveCapturedImage(CaptureRenderedImageAsPNG());
        }

        private byte[] CaptureRenderedImageAsPNG()
        {
            var tex = new Texture2D(_buffer.width, _buffer.height, TextureFormat.RGB24, false);

            var currentRenderTexture = RenderTexture.active;
            RenderTexture.active = _buffer;

            tex.ReadPixels(new Rect(0, 0, _buffer.width, _buffer.height), 0, 0);
            tex.Apply();

            RenderTexture.active = currentRenderTexture;

            var bytes = tex.EncodeToPNG();
            Destroy(tex);

            return bytes;
        }

        private void SaveCapturedImage(byte[] bytes)
        {
            var dir = Path.Combine(Application.persistentDataPath, "Capture");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, $"Screen_{Time.frameCount}.png");
            File.WriteAllBytes(filePath, bytes);
        }
    }
}
