﻿using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class TestClient : MonoBehaviour
    {
        private static readonly Encoding Encoding = new UTF8Encoding(false, false);

        [SerializeField]
        private InputField _addressInput = null;

        [SerializeField]
        private InputField _portInput = null;

        [SerializeField]
        private InputField _commandInput = null;

        [SerializeField]
        private Text _outputTextPrefab = null;

        [SerializeField]
        private Transform _outputTextRoot = null;

        private TcpClient _client;
        private TextWriter _writer;
        private TextReader _reader;

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

        public void Submit()
        {
            if (_client == null)
            {
                AppendLine("not connected");
            }

            var command = _commandInput.text;
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
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}
