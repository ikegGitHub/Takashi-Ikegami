using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class AxisRangeView : MonoBehaviour
    {
        private static Material _sharedMaterial;

        [SerializeField]
        private Image _arcImage = null;

        [SerializeField]
        private Transform _rotator = null;

        private Vector3 _angleMinDirection;
        private Vector3 _angleMaxDirection;
        private Vector3 _currentAngleRatioDirection;

        private float _currentAngleRatio;

        public Vector3 Axis { get; set; } = Vector3.up;

        public float AngleMin { get; set; }

        public float AngleMax { get; set; }

        public float CurrentAngleRatio
        {
            get
            {
                return _currentAngleRatio;
            }
            set
            {
                _currentAngleRatio = Mathf.Clamp01(value);
            }
        }

        private void Awake()
        {
            if (_sharedMaterial == null)
            {
                _sharedMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }
        }

        private void Update()
        {
            transform.localRotation = Quaternion.FromToRotation(Vector3.up, Axis);

            (var min, var max) = (AngleMin, AngleMax);
            if (min > max)
            {
                (min, max) = (max, min);
            }
            _arcImage.fillClockwise = AngleMin > AngleMax;
            _arcImage.fillAmount = Mathf.Clamp01((max - min) / 360.0f);

            var angles = _rotator.localEulerAngles;
            angles.y = AngleMin;
            _rotator.localEulerAngles = angles;

            _angleMinDirection = Quaternion.AngleAxis(AngleMin, transform.up) * transform.right;
            _angleMaxDirection = Quaternion.AngleAxis(AngleMax, transform.up) * transform.right;
            _currentAngleRatioDirection = Quaternion.AngleAxis(Mathf.LerpAngle(AngleMin, AngleMax, _currentAngleRatio), transform.up) * transform.right;
        }

        private void OnRenderObject()
        {
            _sharedMaterial.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);

            GLDrawRay(transform.position, transform.up, Color.red, Color.red);
            GLDrawRay(transform.position, _angleMinDirection, Color.yellow, new Color(1, 1, 0, 0));
            GLDrawRay(transform.position, _angleMaxDirection, Color.yellow, new Color(1, 1, 0, 0));
            GLDrawRay(transform.position, _currentAngleRatioDirection, Color.blue, new Color(0, 0, 1, 0));

            GL.End();
            GL.PopMatrix();
        }

        private static void GLDrawRay(Vector3 position, Vector3 direction, Color startColor, Color endColor)
        {
            GL.Color(startColor);
            GL.Vertex(position);
            GL.Color(endColor);
            GL.Vertex(position + direction);
        }
    }
}
