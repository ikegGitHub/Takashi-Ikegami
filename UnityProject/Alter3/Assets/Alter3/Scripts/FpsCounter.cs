using TMPro;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent, RequireComponent(typeof(TMP_Text))]
    public class FpsCounter : MonoBehaviour
    {
        private const float MeasureInterval = 1.0f;

        private const int BufferLength = 20;

        private readonly float[] _fps = new float[BufferLength];

        private float _lastTime;
        private int _frameCount;
        private int _index;
        private int _availableCount;

        private TMP_Text _fpsText = null;

        private void Start()
        {
            _fpsText = GetComponent<TMP_Text>();
            _lastTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            _frameCount++;

            var nowTime = Time.realtimeSinceStartup;
            var diffTime = nowTime - _lastTime;
            if (diffTime >= MeasureInterval)
            {
                _fps[_index] = _frameCount / diffTime;
                _index = (_index + 1) % BufferLength;

                _lastTime = nowTime;
                _frameCount = 0;

                if (_availableCount < BufferLength)
                {
                    _availableCount++;
                }

                if (_availableCount > 2)
                {
                    float fps = 0f, minFps = float.MaxValue, maxFps = 0f;
                    for (int i = 0; i < _availableCount; i++)
                    {
                        var f = _fps[i];
                        fps += f;
                        minFps = Mathf.Min(f, minFps);
                        maxFps = Mathf.Max(f, maxFps);
                    }
                    fps -= minFps + maxFps;
                    fps /= _availableCount - 2;

                    _fpsText.text = $"{fps:F02} FPS";
                }
            }
        }
    }
}
