using System;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [CreateAssetMenu(menuName = "Alter3/Table/軸初期値テーブル", fileName = "NewInitialAxisValueTable")]
    public class InitialAxisValueTable : ScriptableObject
    {
        [SerializeField, Range(0, 255)]
        private float _defaultValue = 127;

        [SerializeField]
        private OverrideValue[] _overrideValues = null;

        public float GetValue(int axisNumber)
        {
            foreach (var overrideValue in _overrideValues)
            {
                if (axisNumber == overrideValue.AxisNumber)
                {
                    return overrideValue.Value;
                }
            }
            return _defaultValue;
        }

        [Serializable]
        private struct OverrideValue
        {
            public int AxisNumber;

            [Range(0, 255)]
            public float Value;
        }
    }
}
