using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private JointTable _jointTable = null;

        [SerializeField]
        private Button _connectButton = null;

        [SerializeField]
        private Button _clearLogButton = null;

        private TcpClient _client;
        private TextWriter _writer;
        private TextReader _reader;
        private LinkedList<GameObject> _logLines = new LinkedList<GameObject>();
        private int _logCount;

        private void Connect()
        {
            if (_client != null)
            {
                AppendLineError($"already connected ({_client.Client.RemoteEndPoint})");
                return;
            }

            try
            {
                _client = new TcpClient(_addressInput.text, int.Parse(_portInput.text));
                var stream = _client.GetStream();
                _writer = new StreamWriter(stream, Encoding);
                _reader = new StreamReader(stream, Encoding);

                AppendLineSuccess("connected");
            }
            catch (SocketException)
            {
                AppendLineError("failed to connect");
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
                AppendLineError("not connected");
                return;
            }
            if (!_client.Connected)
            {
                Disconnect();
                AppendLineError("connection lost");
                return;
            }

            _commandInput.text = "";
            _commandInput.ActivateInputField();

            AppendLineLog($"(req) {command}");
            _writer.WriteLine(command);
            _writer.Flush();

            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                line = line.Trim();
                AppendLineLog($"(res) {line}");
                if (line == "OK" || line.StartsWith("ERROR: "))
                {
                    break;
                }
            }
        }

        private void AppendLineLog(string line) => AppendLine(line, Color.white);

        private void AppendLineSuccess(string line) => AppendLine(line, Color.green);

        private void AppendLineError(string line) => AppendLine(line, Color.red);

        private void AppendLine(string line, Color color)
        {
            var lineText = Instantiate(_outputTextPrefab, _outputTextRoot, false);
            lineText.text = line;
            lineText.color = color;

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
                axisController.ContentsText = entity.Content;

                // 範囲は仮
                axisController.MinValue = 0;
                axisController.MaxValue = 255;
                axisController.Value = 128;
            }
        }

        private void ClearLog()
        {
            _logLines.Clear();
            _logCount = 0;
            foreach (Transform child in _outputTextRoot)
            {
                Destroy(child.gameObject);
            }
        }

        private void Awake()
        {
            _commandInput.onSubmit.AddListener(Submit);
            InitializeAxisControllers(44);

            _connectButton.onClick.AddListener(() => Connect());
            _clearLogButton.onClick.AddListener(() => ClearLog());
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}
