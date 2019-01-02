using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class LogWindow : MonoBehaviour, ILogHandler
    {
        [SerializeField]
        private TMP_Text _logTextPrefab = null;

        [SerializeField]
        private Transform _logTextRoot = null;

        [SerializeField]
        private Button _closeButton = null;

        [SerializeField]
        private int _logLineLimit = 1000;

        private LinkedList<TMP_Text> _logTexts = new LinkedList<TMP_Text>();
        private int _logCount;

        public bool IsShown => gameObject.activeSelf;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
        }

        private void AppendLogLine(string text, Color color)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            var logText = Instantiate(_logTextPrefab, _logTextRoot, false);
            logText.text = text;
            logText.color = color;
            _logTexts.AddLast(logText);

            if (_logCount >= _logLineLimit)
            {
                RemoveOldestLogText();
            }
            else
            {
                ++_logCount;
            }
        }

        private void RemoveOldestLogText()
        {
            var first = _logTexts.First.Value;
            _logTexts.RemoveFirst();
            Destroy(first.gameObject);
        }

        private Color GetLogColor(LogType logType)
        {
            switch (logType)
            {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    return Color.red;
                case LogType.Warning:
                    return Color.yellow;
            }
            return Color.white;
        }

        void ILogHandler.LogException(Exception exception, Object context)
        {
            AppendLogLine($"[{LogUtil.Timestamp()}] {exception.Message}", GetLogColor(LogType.Exception));
        }

        void ILogHandler.LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            AppendLogLine($"[{LogUtil.Timestamp()}] {string.Format(format, args)}", GetLogColor(logType));
        }
    }
}
