using System;
using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class AxisModel
    {
        public int Id => _masterData.id;

        public bool IsSpring => _masterData.IsSpring;

        public float Spring => _masterData.Spring;

        public float Dumper => _masterData.Dumper;

        public List<JointParameter> Joints { get; } = new List<JointParameter>();

        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = Mathf.Clamp(value, 0, 255);
                OnValueChanged(_value);
            }
        }

        public event Action<float> OnValueChanged = delegate { };

        private readonly JointTableEntity _masterData;

        private float _value = 128;

        public AxisModel(JointTableEntity masterData)
        {
            _masterData = masterData;
        }

        public void ClearEventHandler()
        {
            OnValueChanged = delegate { };
        }
    }
}
