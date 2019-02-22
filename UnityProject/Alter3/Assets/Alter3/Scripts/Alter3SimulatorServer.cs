using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using XFlag.Alter3Simulator.Network;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class Alter3SimulatorServer : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _sampleConfig = null;

        [SerializeField]
        private string _listenAddress = "";

        [SerializeField]
        private TMP_Text _serverStatusText = null;

        [SerializeField]
        private TMP_InputField _portInputField = null;

        [SerializeField]
        private TMP_Text _clientListText = null;

        [SerializeField]
        private ServerToggleView _serverToggle = null;

        [SerializeField]
        private RobotSimulatorBaseController _robot = null;

        [SerializeField]
        private TMP_InputField _logFileLocation = null;

        [SerializeField]
        private RawImage _LeftEyeRawImage = null;

        [SerializeField]
        private RawImage _RightEyeRawImage = null;

        [SerializeField]
        private GameObject _faceCameraScreen = null;

        [SerializeField]
        private CameraController _cameraController = null;

        [SerializeField]
        private AxisControlPanel _axisControlPanel = null;

        private ILogger _logger;

        private IDictionary<string, string> _config;
        private CoreSystem _coreSystem = new CoreSystem(50);
        private ConnectionManager _server;

        private SynchronizationContext _context;

        private void Awake()
        {
            Assert.IsNotNull(_sampleConfig);
            Assert.IsNotNull(_serverStatusText);
            Assert.IsNotNull(_clientListText);
            Assert.IsNotNull(_serverToggle);
            Assert.IsNotNull(_robot);
            Assert.IsNotNull(_logFileLocation);
            Assert.IsNotNull(_LeftEyeRawImage);
            Assert.IsNotNull(_RightEyeRawImage);
            Assert.IsNotNull(_faceCameraScreen);
            Assert.IsNotNull(_cameraController);
            Assert.IsNotNull(_axisControlPanel);

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

            _serverToggle.OnValueChanged += isOn => OnServerButtonClick(isOn);
        }

        private void Start()
        {
            _LeftEyeRawImage.texture = _robot.EyeCameraLeft.RenderTexture;
            _RightEyeRawImage.texture = _robot.EyeCameraRight.RenderTexture;

            _axisControlPanel.Initialize(_robot);

            _serverToggle.IsOn = true;
        }

        private void Update()
        {
            if (!_faceCameraScreen.activeSelf && Input.GetKey(KeyCode.F))
            {
                _faceCameraScreen.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                // Pキーでカメラリセット
                _cameraController.ResetPosition();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                // Oキーで斜め前回り込み
                _cameraController.MoveToForwardOfTarget();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                // Rキーでロボットリセット
                _robot.ResetAxes();
            }
            else if (!_axisControlPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Alpha9))
            {
                // 9キーで軸コントロールパネル表示
                _axisControlPanel.gameObject.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _server.StopServer();
        }

        private void OnServerButtonClick(bool start)
        {
            try
            {
                if (start)
                {
                    StartServer();
                }
                else
                {
                    StopServer();
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                _serverStatusText.text = e.Message;
            }
        }

        private void StartServer()
        {
            var port = ushort.Parse(_portInputField.text);
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
            return new Logger(Debug.unityLogger.And(new FileLogger(logFileName)));
        }
    }
}
