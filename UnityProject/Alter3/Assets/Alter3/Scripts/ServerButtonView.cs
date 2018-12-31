using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ServerButtonView : MonoBehaviour
    {
        private static readonly string StartText = "Start Server";

        private static readonly string StopText = "Stop Server";

        [SerializeField]
        private Button _button = null;

        [SerializeField]
        private TMP_Text _text = null;

        public Button.ButtonClickedEvent OnClick => _button.onClick;

        public bool IsStart
        {
            get
            {
                return _isStart;
            }
            set
            {
                _isStart = value;
                if (_isStart)
                {
                    _text.text = StartText;
                }
                else
                {
                    _text.text = StopText;
                }
            }
        }

        private bool _isStart;
    }
}
