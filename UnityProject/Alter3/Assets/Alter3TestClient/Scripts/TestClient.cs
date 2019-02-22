using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class TestClient : MonoBehaviour
    {
        private const int AxisCount = 43;

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
        private ButtonView _connectButton = null;

        [SerializeField]
        private Button _clearLogButton = null;

        [SerializeField]
        private Button _sendAllButton = null;

        [SerializeField]
        private SelectValueSetFileView _selectPresetFileView = null;

        private SynchronizationContext _syncContext;

        private TcpClient _client;
        private TextWriter _writer;
        private Task _readTask;
        private LinkedList<GameObject> _logLines = new LinkedList<GameObject>();
        private int _logCount;

        private List<AxisControllerView> _axisSliders = new List<AxisControllerView>(AxisCount);

        private bool IsConnected => _client != null && _client.Connected;

        private void ToggleConnect()
        {
            if (IsConnected)
            {
                Disconnect();
            }
            else
            {
                Connect();
            }
        }

        private void Connect()
        {
            if (IsConnected)
            {
                AppendLineError($"already connected ({_client.Client.RemoteEndPoint})");
                return;
            }

            try
            {
                _client = new TcpClient(_addressInput.text, int.Parse(_portInput.text));
                var stream = _client.GetStream();
                _writer = new StreamWriter(stream, Encoding);
                Task.Factory.StartNew(async () => await ReadResponseAsync(_client), TaskCreationOptions.LongRunning);

                AppendLineSuccess("connected");
                _connectButton.ButtonText = "disconnect";
            }
            catch (SocketException)
            {
                AppendLineError("failed to connect");
            }
        }

        private void Disconnect()
        {
            if (_client != null)
            {
                _client.Close();
                _client = null;
                _writer = null;
            }
            _connectButton.ButtonText = "connect";
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
        }

        private async Task ReadResponseAsync(TcpClient tcpClient)
        {
            try
            {
                using (var reader = new StreamReader(tcpClient.GetStream(), Encoding))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        line = line.Trim();
                        AppendLineLog($"(res) {line}");
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void AppendLineLog(string line) => AppendLine(line, Color.white);

        private void AppendLineSuccess(string line) => AppendLine(line, Color.green);

        private void AppendLineError(string line) => AppendLine(line, Color.red);

        private void AppendLine(string line, Color color)
        {
            _syncContext.Post(state => AppendLineInternal(line, color), null);
        }

        private void AppendLineInternal(string line, Color color)
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

                _axisSliders.Add(axisController);
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

        private void SendAll()
        {
            for (int i = 0; i < _axisSliders.Count; i++)
            {
                var axisNumber = i + 1;
                var value = _axisSliders[i].Value;
                SendMoveAxisCommand(axisNumber, value);
            }
        }

        private void LoadPresetFile(string filePath)
        {
            using (var parser = new SimpleCsvParser(new StreamReader(filePath, Encoding.UTF8)))
            {
                var rows = parser.Parse();
                foreach (var row in rows)
                {
                    var axisNumber = int.Parse(row[0]);
                    var value = float.Parse(row[1]);
                    _axisSliders[axisNumber - 1].Value = value;
                }
            }
        }

        private void Awake()
        {
            _syncContext = SynchronizationContext.Current;

            _commandInput.onSubmit.AddListener(Submit);
            InitializeAxisControllers(AxisCount);

            _connectButton.OnClick += ToggleConnect;
            _clearLogButton.onClick.AddListener(() => ClearLog());
            _sendAllButton.onClick.AddListener(() => SendAll());
            _selectPresetFileView.OnFileSelected += LoadPresetFile;
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}
