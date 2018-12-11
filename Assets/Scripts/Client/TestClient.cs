using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
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
            _client = new TcpClient(_addressInput.text, int.Parse(_portInput.text));
            var stream = _client.GetStream();
            _writer = new StreamWriter(stream, Encoding);
            _reader = new StreamReader(stream, Encoding);

            AppendLine("connected");
        }

        public void Disconnect()
        {
            _client.Close();
        }

        public void Submit()
        {
            var command = _commandInput.text;
            _commandInput.text = "";

            _writer.WriteLine(command);
            _writer.Flush();

            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                line = line.Trim();
                AppendLine(line);
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
