﻿using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class TestClient : MonoBehaviour
    {
        private static readonly Encoding Encoding = new UTF8Encoding(false, false);

        [SerializeField]
        private TMP_InputField _addressInput = null;

        [SerializeField]
        private TMP_InputField _portInput = null;

        [SerializeField]
        private TMP_InputField _commandInput = null;

        [SerializeField]
        private TMP_Text _outputTextPrefab = null;

        [SerializeField]
        private Transform _outputTextRoot = null;

        [SerializeField]
        private AxisControllerView _axisControllerPrefab = null;

        [SerializeField]
        private Transform _axisControllerRoot = null;

        [SerializeField]
        JointTable _jointTable = null;


        private TcpClient _client;
        private TextWriter _writer;
        private TextReader _reader;
        private LinkedList<GameObject> _logLines = new LinkedList<GameObject>();
        private int _logCount;

        public void Connect()
        {
            if (_client != null)
            {
                return;
            }

            try
            {
                _client = new TcpClient(_addressInput.text, int.Parse(_portInput.text));
                var stream = _client.GetStream();
                _writer = new StreamWriter(stream, Encoding);
                _reader = new StreamReader(stream, Encoding);

                AppendLine("connected");
            }
            catch (SocketException)
            {
                AppendLine("failed to connect");
            }
        }

        public void Disconnect()
        {
            if (_client != null)
            {
                _client.Close();
                _client = null;
                _writer = null;
                _reader = null;
            }
        }

        public void Submit(string command)
        {
            if (_client == null)
            {
                AppendLine("not connected");
                return;
            }

            _commandInput.text = "";
            _commandInput.ActivateInputField();

            AppendLine($"(req) {command}");
            _writer.WriteLine(command);
            _writer.Flush();

            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                line = line.Trim();
                AppendLine($"(res) {line}");
                if (line == "OK" || line.StartsWith("ERROR: "))
                {
                    break;
                }
            }
        }

        private void AppendLine(string line)
        {
            var lineText = Instantiate(_outputTextPrefab, _outputTextRoot, false);
            lineText.text = line;

            _logLines.AddLast(lineText.gameObject);
            if (_logCount < 100)
            {
                ++_logCount;
            }
            else
            {
                var oldestObject = _logLines.First.Value;
                _logLines.RemoveFirst();
                Destroy(oldestObject);
            }
        }

        private void SendMoveAxisCommand(int axisNumber, float value)
        {
            Submit($"MOVEAXIS {axisNumber} {value}");
        }

        private void InitializeAxisControllers(int axisCount)
        {
            for (var i = 1; i <= axisCount; i++)
            {
                var axisNumber = i;
                var axisController = Instantiate(_axisControllerPrefab, _axisControllerRoot, false);
                axisController.OnValueChanged += value => SendMoveAxisCommand(axisNumber, value);
                axisController.LabelText = i.ToString();
                var entity = _jointTable.GetEntity(i);
                axisController.JointNameText = entity.Name;

                // 範囲は仮
                axisController.MinValue = 0;
                axisController.MaxValue = 255;
            }
        }

        private void Awake()
        {
            _commandInput.onSubmit.AddListener(Submit);
            InitializeAxisControllers(44);
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}
