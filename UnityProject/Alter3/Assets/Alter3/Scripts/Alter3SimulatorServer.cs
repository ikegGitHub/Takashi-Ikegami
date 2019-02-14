using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XFlag.Alter3Simulator.Network;

namespace XFlag.Alter3Simulator
{
    public class Alter3SimulatorServer : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _sampleConfig = null;

        [SerializeField]
        private string _listenAddress = "";

        [SerializeField]
        private TMP_Text _serverStatusText = null;

        [SerializeField]
        private TMP_Text _clientListText = null;

        [SerializeField]
        private LampView _serverStatusLamp = null;

        [SerializeField]
        private LogWindow _logWindow = null;

        [SerializeField]
        private RobotSimulatorBaseController _robot = null;

        [SerializeField]
        private TMP_InputField _logFileLocation = null;

        [SerializeField]
        private RawImage _LeftEyeRawImage = null;

        [SerializeField]
        private RawImage _RightEyeRawImage = null;


        private ILogger _logger;

        private IDictionary<string, string> _config;
        private CoreSystem _coreSystem = new CoreSystem(50);
        private ConnectionManager _server;

        private SynchronizationContext _context;

        private float _keyDownTime;

        private void Awake()
        {
            _context = SynchronizationContext.Current;

            _config = ConfigParser.Parse(_sampleConfig);

            _logger = CreateLogger();

            _coreSystem.Robot = _robot;

            _server = new ConnectionManager(new IncrementalSequencer())
            {
                Logger = _logger
            };

            _server.OnConnected += OnClientConnected;
            _server.OnDisconnected += OnClientDisconnected;
            _server.OnReceived += OnReceived;


            _serverStatusLamp.OnClick += eventData => OnServerButtonClick();
        }

        private void Start()
        {
            OnServerButtonClick();
        }

        private void Update()
        {
            // Lボタンを長押しでログウィンドウ表示
            if (!_logWindow.IsShown && Input.GetKey(KeyCode.L))
            {
                if (_keyDownTime >= 3.0f)
                {
                    _logWindow.Show();
                }
                else
                {
                    _keyDownTime += Time.deltaTime;
                }
            }
            else
            {
                _keyDownTime = 0;
            }

            _LeftEyeRawImage.texture = _robot.EyeCameraLeft.RenderTexture;
            _RightEyeRawImage.texture = _robot.EyeCameraRight.RenderTexture;
        }

        private void OnDestroy()
        {
            _server.StopServer();
        }

        private void OnServerButtonClick()
        {
            _serverStatusLamp.IsError = false;

            try
            {
                if (_server.IsStarted)
                {
                    StopServer();
                }
                else
                {
                    StartServer();
                }
                _serverStatusLamp.IsOn = _server.IsStarted;
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                _serverStatusText.text = e.Message;
                _serverStatusLamp.IsError = true;
            }
        }

        private void StartServer()
        {
            var port = ushort.Parse(_config["port_num_User"]);
            _server.StartServerAsync(_listenAddress, port);
            _serverStatusText.text = $"server started {_listenAddress}:{port}";
        }

        private void StopServer()
        {
            _server.StopServer();
            _serverStatusText.text = "server stopped";
        }

        private void UpdateClientListText()
        {
            _context.Post(state =>
            {
                _clientListText.text = _coreSystem.Clients.Aggregate("", (str, c) => $"{str}\n{c}");
            }, null);
        }

        private void OnClientConnected(uint clientId, IPEndPoint ipEndPoint)
        {
            _coreSystem.RegisterClient(clientId, ipEndPoint.Address, ClientType.User);
            UpdateClientListText();
        }

        private void OnClientDisconnected(uint clientId)
        {
            _coreSystem.UnregisterClient(clientId);
            UpdateClientListText();
        }

        private void OnReceived(RequestContext context)
        {
            new CommandProcessor(_coreSystem, context).ProcessCommand();
        }

        private ILogger CreateLogger()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var logFileName = Path.Combine(Path.Combine(Application.persistentDataPath, "Logs"), $"{timestamp}.log");
            _logFileLocation.text = logFileName;
            return new Logger(Debug.unityLogger.And(_logWindow).And(new FileLogger(logFileName)));
        }
    }
}
