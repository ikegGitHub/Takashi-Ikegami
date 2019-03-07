using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class ScreenCaptureCommandBuffer : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int _resolution = new Vector2Int(256, 256);

        [SerializeField]
        private int _frameInterval = 3;

        [SerializeField]
        private CameraEvent _when = CameraEvent.AfterEverything;

        private Camera _camera;
        private Vector2Int _currentResolution;
        private RenderTexture _buffer;
        private CommandBuffer _commandBuffer;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _currentResolution = new Vector2Int(_camera.pixelWidth, _camera.pixelHeight);
        }

        private void OnEnable()
        {
            InitializeCommandBuffer();
        }

        private void OnDisable()
        {
            DisposeCommandBuffer();
        }

        private void OnPostRender()
        {
            if (Time.frameCount % _frameInterval == 0)
            {
                RequestCopyRenderTexture(_buffer);
            }
        }

        private void OnPreRender()
        {
            if (enabled)
            {
                if (_currentResolution.x != _camera.pixelWidth || _currentResolution.y != _camera.pixelHeight)
                {
                    _currentResolution = new Vector2Int(_camera.pixelWidth, _camera.pixelHeight);
                    ReinitializeCommandBuffer();
                }
            }
        }

        private void InitializeCommandBuffer()
        {
            if (_buffer != null || _commandBuffer != null)
            {
                return;
            }

            _buffer = new RenderTexture(_resolution.x, _resolution.y, 0, RenderTextureFormat.ARGB32);
            _buffer.Create();

            var offset = Vector2.zero;
            var scale = Vector2.one;
            if (_currentResolution.x >= _currentResolution.y)
            {
                scale = new Vector2((float)_currentResolution.y / _currentResolution.x, 1);
                offset = new Vector2(0.5f * (1 - (float)_currentResolution.y / _currentResolution.x), 0);
            }
            else
            {
                scale = new Vector2(1, (float)_currentResolution.x / _currentResolution.y);
                offset = new Vector2(0, 0.5f * (1 - (float)_currentResolution.x / _currentResolution.y));
            }

            _commandBuffer = new CommandBuffer { name = "ScreenCapture" };
            _commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, _buffer, scale, offset);

            _camera.AddCommandBuffer(_when, _commandBuffer);
        }

        private void ReinitializeCommandBuffer()
        {
            DisposeCommandBuffer();
            InitializeCommandBuffer();
        }

        private void DisposeCommandBuffer()
        {
            if (_commandBuffer != null)
            {
                _camera.RemoveCommandBuffer(_when, _commandBuffer);
                _commandBuffer.Dispose();
                _commandBuffer = null;
            }
            if (_buffer != null)
            {
                Destroy(_buffer);
                _buffer = null;
            }
        }

        private void RequestCopyRenderTexture(RenderTexture renderTexture)
        {
            AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGB24, request =>
            {
                if (enabled)
                {
                    var rawData = request.GetData<Color32>();
                    var tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
                    tex.LoadRawTextureData(rawData);
                    tex.Apply();

                    var bytes = tex.EncodeToPNG();
                    Destroy(tex);

                    SaveCapturedImage(bytes);
                }
            });
        }

        private static void SaveCapturedImage(byte[] bytes)
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
