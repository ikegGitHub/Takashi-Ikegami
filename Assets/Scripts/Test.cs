﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _sampleConfig = null;

        private IDictionary<string, string> _config;
        private CoreSystem _coreSystem = new CoreSystem(50);
        private ConnectionManager _server = new ConnectionManager();

        private void Awake()
        {
            _config = ConfigParser.Parse(_sampleConfig);

            _server.OnConnected += (id, endPoint) =>
            {
                _coreSystem.RegisterClient(id, endPoint.Address, ClientType.User);
            };
            _server.OnDisconnected += _coreSystem.UnregisterClient;
            _server.OnReceived += OnReceived;
        }

        private void OnDestroy()
        {
            StopListen();
        }

        [ContextMenu("Start Listen")]
        private async Task StartListen()
        {
            var port = ushort.Parse(_config["port_num_User"]);
            await _server.StartServerAsync("0.0.0.0", port).ConfigureAwait(false);
        }

        [ContextMenu("Stop Listen")]
        private void StopListen()
        {
            _server.StopServer();
        }

        private void OnReceived(RequestContext context)
        {
            var parser = new CommandParser();
            var processor = new CommandProcessor(_coreSystem, context.ClientId);
            try
            {
                var command = parser.ParseCommandLine(context.ReceivedString);
                foreach (var responseLine in command.AcceptVisitor(processor))
                {
                    Debug.Log($"[{context.ClientId}] {responseLine}");
                    context.ResponseWriter.WriteLine(responseLine);
                }
                if (command is QuitCommand)
                {
                    context.IsClose = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"[{context.ClientId}] respond: ERROR: {e.Message}");
                context.ResponseWriter.WriteLine(Response.MakeErrorResponse(e.Message));
            }
            context.ResponseWriter.Flush();
        }
    }
}
