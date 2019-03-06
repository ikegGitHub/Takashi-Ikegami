﻿using System;
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
        private ClientConnectionListView _clientConnectionListView = null;

        [SerializeField]
        private ServerToggleView _serverToggle = null;

        [SerializeField]
        private RobotSimulatorBaseController _robot = null;

        [SerializeField]
        private TMP_InputField _logFileLocation = null;

        [SerializeField]
        private GameObject _eyeCameraScreen = null;

        [SerializeField]
        private RawImage _LeftEyeRawImage = null;

        [SerializeField]
        private RawImage _RightEyeRawImage = null;

        [SerializeField]
        private GameObject _faceCameraScreen = null;

        [SerializeField]
        private CameraController _cameraController = null;

        [SerializeField]
        private FaceCameraController _faceCameraController = null;

        [SerializeField]
        private AxisControlPanel _axisControlPanel = null;

        [SerializeField]
        private TextAsset _playData = null;

        [SerializeField]
        private TMP_InputField _systemPortInputField = null;

        [SerializeField]
        private ControlPanelView _controlPanelView = null;

        private ILogger _logger;

        private IDictionary<string, string> _config;
        private CoreSystem _coreSystem = new CoreSystem(50);
        private ConnectionManager _server;
        private ConnectionManager _systemCommandConnection;
        private SettingModel _setting = new SettingModel();

        private SynchronizationContext _context;

        private CommandTimelinePlayer _player;

        private void Awake()
        {
            Assert.IsNotNull(_sampleConfig);
            Assert.IsNotNull(_serverStatusText);
            Assert.IsNotNull(_clientConnectionListView);
            Assert.IsNotNull(_serverToggle);
            Assert.IsNotNull(_robot);
            Assert.IsNotNull(_logFileLocation);
            Assert.IsNotNull(_LeftEyeRawImage);
            Assert.IsNotNull(_RightEyeRawImage);
            Assert.IsNotNull(_faceCameraScreen);
            Assert.IsNotNull(_cameraController);
            Assert.IsNotNull(_faceCameraController);
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

            _systemCommandConnection = new ConnectionManager(new IncrementalSequencer())
            {
                Logger = _logger
            };
            _systemCommandConnection.OnReceived += request =>
            {
                switch (request.ReceivedString.ToUpper())
                {
                    case "GETHANDSPOS": // 両手の座標取得
                        {
                            var handsPositions = _robot.GetHandsPositionArray();
                            request.ResponseLines = new string[] { $"{handsPositions[0].x} {handsPositions[0].y} {handsPositions[0].z} {handsPositions[1].x} {handsPositions[1].y} {handsPositions[1].z}" };
                        }
                        break;
                }
            };

            _serverToggle.OnValueChanged += isOn => OnServerButtonClick(isOn);

            _cameraController.LookTarget = _robot.FindName("Hips").transform;
            _faceCameraController.LookTarget = _robot.FindName("Head").transform;
        }

        private void Start()
        {
            _LeftEyeRawImage.texture = _robot.EyeCameraLeft.RenderTexture;
            _RightEyeRawImage.texture = _robot.EyeCameraRight.RenderTexture;

            _axisControlPanel.Initialize(_robot);

            _serverToggle.IsOn = true;

            _controlPanelView.OnEnableEyeCameraChanged += isOn => _setting.EnableEyeCamera = isOn;
            _controlPanelView.OnEnableFaceCameraChanged += isOn => _setting.EnableFaceCamera = isOn;
            _controlPanelView.OnEnableCollisionCheckChanged += isOn => _setting.EnableCollisionCheck = isOn;
            _controlPanelView.OnEnableClothModelChanged += isOn => _setting.EnableClothModel = isOn;
            _controlPanelView.OnResetCameraButtonClicked += () => _cameraController.ResetPosition();
            _controlPanelView.OnResetPositionButtonClicked += () => _robot.ResetAxes();

            _setting.OnEnableEyeCameraChanged += enabled =>
            {
                _eyeCameraScreen.SetActive(enabled);
                _controlPanelView.EnableEyeCamera = enabled;
            };
            _setting.OnEnableFaceCameraChanged += enabled =>
            {
                _faceCameraScreen.SetActive(enabled);
                _controlPanelView.EnableFaceCamera = enabled;
            };
            _setting.OnEnableCollisionCheckChanged += enabled =>
            {
                _robot.SetCollisionCheckEnabled(enabled);
                _controlPanelView.EnableCollisionCheck = enabled;
            };
            _setting.OnEnableClothModelChanged += enabled =>
            {
                _robot.SetClothModelEnabled(enabled);
                _controlPanelView.EnableClothModel = enabled;
            };

            _eyeCameraScreen.SetActive(_setting.EnableEyeCamera);
            _controlPanelView.EnableEyeCamera = _setting.EnableEyeCamera;

            _faceCameraScreen.SetActive(_setting.EnableFaceCamera);
            _controlPanelView.EnableFaceCamera = _setting.EnableFaceCamera;

            _robot.SetCollisionCheckEnabled(_setting.EnableCollisionCheck);
            _controlPanelView.EnableCollisionCheck = _setting.EnableCollisionCheck;

            _robot.SetClothModelEnabled(_setting.EnableClothModel);
            _controlPanelView.EnableClothModel = _setting.EnableClothModel;

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                // Oキーで斜め前回り込み
                _cameraController.MoveToForwardOfTarget();
            }
            else if (!_axisControlPanel.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Alpha9))
            {
                // 9キーで軸コントロールパネル表示
                _axisControlPanel.gameObject.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                // 2/23版ScaryBeautyデータ再生
                TogglePlayData();
            }
        }

        private void OnDestroy()
        {
            _server.StopServer();
            _systemCommandConnection.StopServer();
            StopPlayerData();
        }

        /// <summary>
        /// 2/23版のMIDIデータから発行されるコマンドタイムラインを再生する
        /// </summary>
        private void TogglePlayData()
        {
            if (_player != null)
            {
                StopPlayerData();
                return;
            }

            using (var loader = new CommandTimelineDataLoader(new BinaryReader(new MemoryStream(_playData.bytes))))
            {
                var commands = loader.Load();
                _player = new CommandTimelinePlayer(commands);
            }

            _player.OnCommand += command =>
            {
                _context.Post(state => _robot.MoveAxis(command.Param), null);
            };
            _player.Start();
        }

        private void StopPlayerData()
        {
            if (_player != null)
            {
                _player.Stop();
                _player = null;
            }
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

            var systemPort = ushort.Parse(_systemPortInputField.text);
            _systemCommandConnection.StartServerAsync("0.0.0.0", systemPort);

            _serverStatusText.text = $"server started {_listenAddress}:{port} / system port = {systemPort}";
        }

        private void StopServer()
        {
            _systemCommandConnection.StopServer();
            _server.StopServer();
            _serverStatusText.text = "server stopped";
        }

        private void OnClientConnected(uint clientId, IPEndPoint ipEndPoint)
        {
            _coreSystem.RegisterClient(clientId, ipEndPoint.Address, ClientType.User);
            _context.Post(state => _clientConnectionListView.Add(clientId, ipEndPoint), null);
        }

        private void OnClientDisconnected(uint clientId)
        {
            _coreSystem.UnregisterClient(clientId);
            _context.Post(state => _clientConnectionListView.Remove(clientId), null);
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
