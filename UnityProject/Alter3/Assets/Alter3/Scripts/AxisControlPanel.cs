using System.Collections.Generic;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class AxisControlPanel : MonoBehaviour
    {
        [SerializeField]
        private AxisDialView _axisDialViewPrefab = null;

        [SerializeField]
        private RectTransform _contentRoot = null;

        [SerializeField]
        private JointTable _jointTable = null;

        private RobotSimulatorBaseController _robotController;
        private bool _suppressApplyValue;

        private readonly Dictionary<int, AxisDialView> _axisDials = new Dictionary<int, AxisDialView>();

        public void Initialize(RobotSimulatorBaseController robotController)
        {
            _robotController = robotController;

            _axisDials.Clear();
            foreach (var entry in _jointTable.List)
            {
                var dialView = Instantiate(_axisDialViewPrefab, _contentRoot, false);
                dialView.Initialize(entry.id, entry.Name, 0, 255, 0);
                dialView.OnValueChanged += value =>
                {
                    if (!_suppressApplyValue)
                    {
                        _robotController.MoveAxis(new AxisParam { AxisNumber = entry.id, Value = value });
                    }
                };
                _axisDials.Add(entry.id, dialView);
            }
        }

        private void OnEnable()
        {
            UpdateAxisValues();
        }

        private void Update()
        {
            UpdateAxisValues();
        }

        private void UpdateAxisValues()
        {
            _suppressApplyValue = true;
            foreach (var axisDialEntry in _axisDials)
            {
                (var axisNumber, var axisDial) = (axisDialEntry.Key, axisDialEntry.Value);
                axisDial.Value = _robotController.GetAxisValue(axisNumber);
            }
            _suppressApplyValue = false;
        }
    }
}
