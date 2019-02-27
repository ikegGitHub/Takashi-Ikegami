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
    public class Connection
    {
        private const int PollTimeoutMicroSeconds = 100_000; // 100 [ms]

        private readonly string _hostName;
        private readonly int _port;
        private readonly Encoding _encoding;

        private Socket _socket;
        private NetworkStream _stream;
        private StreamWriter _writer;
        private Task _readTask;

        public event Action OnDisconnected = delegate { };

        public bool IsActive { get; set; } = true;

        public Connection(string hostName, int port, Encoding encoding)
        {
            _hostName = hostName;
            _port = port;
            _encoding = encoding;
        }

        public void Connect()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(_hostName, _port);
            _stream = new NetworkStream(_socket);
            _writer = new StreamWriter(_stream, _encoding);
        }

        public void StartReadingStream()
        {
            _readTask = Task.Factory.StartNew(async () => await ReadResponseAsync(), TaskCreationOptions.LongRunning);
        }

        public void SendLine(string line)
        {
            if (IsActive)
            {
                _writer.WriteLine(line);
            }
        }

        public void Flush()
        {
            if (IsActive)
            {
                _writer.Flush();
            }
        }

        public void Close()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Disconnect(false);
            _socket.Close();
        }

        private async Task ReadResponseAsync()
        {
            try
            {
                using (var reader = new StreamReader(_stream, _encoding))
                {
                    while (_socket.Connected)
                    {
                        if (_socket.Poll(PollTimeoutMicroSeconds, SelectMode.SelectRead))
                        {
                            if (!_socket.Connected)
                            {
                                break;
                            }
                            var line = await reader.ReadLineAsync();
                            if (line == null)
                            {
                                break;
                            }
                        }
                        await Task.Yield();
                    }
                }
            }
            finally
            {
                OnDisconnected();
            }
        }
    }

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

        [SerializeField]
        private ConnectionView _connectionViewPrefab = null;

        [SerializeField]
        private RectTransform _connectionListRoot = null;

        private SynchronizationContext _syncContext;

        private List<Connection> _connections = new List<Connection>();
        private LinkedList<GameObject> _logLines = new LinkedList<GameObject>();
        private int _logCount;

        private List<AxisControllerView> _axisSliders = new List<AxisControllerView>(AxisCount);

        private void Connect()
        {
            var hostname = _addressInput.text;
            if (!short.TryParse(_portInput.text, out var port))
            {
                AppendLineError("invalid port");
                return;
            }
            var endPoint = $"{hostname}:{port}";

            try
            {
                var connection = new Connection(hostname, port, Encoding);
                connection.Connect();
                _connections.Add(connection);

                var connectionView = Instantiate(_connectionViewPrefab, _connectionListRoot, false);
                connectionView.Text = endPoint;
                connectionView.OnDisconnectButtonClicked += () => connection.Close();
                connectionView.OnActiveChanged += isActive => connection.IsActive = isActive;

                connection.OnDisconnected += () => OnDisconnected(connection, connectionView);
                connection.StartReadingStream();

                AppendLineSuccess($"connected {endPoint}");
            }
            catch (SocketException)
            {
                AppendLineError("failed to connect");
            }
        }

        private void OnDisconnected(Connection connection, ConnectionView view)
        {
            _syncContext.Post(state =>
            {
                Destroy(view.gameObject);
                _connections.Remove(connection);
            }, null);
        }

        private void Submit(string command)
        {
            AppendLineLog($"(req) {command}");
            foreach (var connection in _connections)
            {
                connection.SendLine(command);
            }
        }

        private void FlushAll()
        {
            foreach (var connection in _connections)
            {
                connection.Flush();
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
                axisController.OnValueChanged += value =>
                {
                    SendMoveAxisCommand(axisNumber, value);
                    FlushAll();
                };
                axisController.LabelText = i.ToString();
                var entity = _jointTable.GetEntity(i);
                axisController.JointNameText = entity.Name;
                axisController.ContentsText = entity.Content;

                // 範囲は仮
                axisController.MinValue = 0;
                axisController.MaxValue = 255;
                axisController.Value = 127;

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
            FlushAll();
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

            InitializeAxisControllers(AxisCount);

            _connectButton.OnClick += Connect;
            _clearLogButton.onClick.AddListener(() => ClearLog());
            _sendAllButton.onClick.AddListener(() => SendAll());
            _selectPresetFileView.OnFileSelected += LoadPresetFile;
        }

        private void OnDestroy()
        {
            foreach (var connection in _connections)
            {
                connection.Close();
            }
        }
    }
}
