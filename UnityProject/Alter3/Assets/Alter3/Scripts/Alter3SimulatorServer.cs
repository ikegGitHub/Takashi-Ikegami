using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using TMPro;
using UnityEngine;

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

        private ILogger _logger;

        private IDictionary<string, string> _config;
        private CoreSystem _coreSystem = new CoreSystem(50);
        private ConnectionManager _server;
        private CommandParser _commandParser = new CommandParser();

        private SynchronizationContext _context;

        private void Awake()
        {
            _context = SynchronizationContext.Current;

            _config = ConfigParser.Parse(_sampleConfig);

            _logger = CreateLogger();

            _server = new ConnectionManager(new IncrementalSequencer())
            {
                Logger = _logger
            };

            _server.OnConnected += OnClientConnected;
            _server.OnDisconnected += OnClientDisconnected;
            _server.OnReceived += OnReceived;
        }

        private void OnDestroy()
        {
            _server.StopServer();
        }

        public void StartListen()
        {
            var port = ushort.Parse(_config["port_num_User"]);
            _server.StartServerAsync(_listenAddress, port);
            _serverStatusText.text = $"server started {_listenAddress}:{port}";
            _serverStatusLamp.IsOn = true;
        }

        public void StopListen()
        {
            _server.StopServer();
            _serverStatusText.text = "server stopped";
            _serverStatusLamp.IsOn = false;
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
            var processor = new CommandProcessor(_coreSystem, context);
            try
            {
                var command = _commandParser.ParseCommandLine(context.ReceivedString);
                foreach (var responseLine in command.AcceptVisitor(processor))
                {
                    context.AppendResponseLine(responseLine);
                }
            }
            catch (Exception e)
            {
                context.AppendResponseLine(Response.MakeErrorResponse(e.Message));
            }
        }

        private ILogger CreateLogger()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var logFileName = $"Logs/{timestamp}.log";
            return new Logger(Debug.unityLogger.And(new FileLogger(logFileName)));
        }
    }
}
