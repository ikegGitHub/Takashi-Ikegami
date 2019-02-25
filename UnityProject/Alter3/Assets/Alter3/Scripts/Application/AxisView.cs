using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class AxisView : MonoBehaviour
    {
        [SerializeField]
        private Image _arcImage = null;

        [SerializeField]
        private Transform _rotator = null;

        private float _angleMin = 0;

        private float _angleMax = 0;

        private Material _material;

        private Vector3 _axis = Vector3.up;

        private float _currentAngleRatio;

        public Vector3 Axis
        {
            get
            {
                return _axis;
            }
            set
            {
                _axis = value;
                transform.localRotation = Quaternion.FromToRotation(Vector3.up, _axis);
            }
        }

        public float AngleMin
        {
            get
            {
                return _angleMin;
            }
            set
            {
                _angleMin = value;
            }
        }

        public float AngleMax
        {
            get
            {
                return _angleMax;
            }
            set
            {
                _angleMax = value;
            }
        }

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
            _material = new Material(Shader.Find("Hidden/Internal-Colored"));
            _material.hideFlags = HideFlags.HideAndDontSave;
        }

        private void Update()
        {
            transform.localRotation = Quaternion.FromToRotation(Vector3.up, _axis);

            (var min, var max) = (_angleMin, _angleMax);
            if (min > max)
            {
                (min, max) = (max, min);
            }
            _arcImage.fillClockwise = _angleMin > _angleMax;
            _arcImage.fillAmount = Mathf.Repeat((max - min) / 360.0f, 1.0f);

            var angles = _rotator.localEulerAngles;
            angles.y = _angleMin;
            _rotator.localEulerAngles = angles;
        }

        private void OnRenderObject()
        {
            _material.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);

            GLDrawRay(transform.position, transform.up, Color.blue, Color.blue);
            GLDrawRay(transform.position, Quaternion.AngleAxis(_angleMin, transform.up) * transform.right, Color.yellow, new Color(1, 1, 0, 0));
            GLDrawRay(transform.position, Quaternion.AngleAxis(_angleMax, transform.up) * transform.right, Color.yellow, new Color(1, 1, 0, 0));
            GLDrawRay(transform.position, Quaternion.AngleAxis(Mathf.LerpAngle(_angleMin, _angleMax, _currentAngleRatio), transform.up) * transform.right, Color.blue, new Color(0, 0, 1, 0));

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
