using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class ConnectionManager
    {
        public ILogger Logger { get; set; }

        private ISequencer _clientIdSequencer;

        private TcpListener _listener;
        private Dictionary<uint, ClientConnectionManager> _clients = new Dictionary<uint, ClientConnectionManager>();

        public event Action<uint, IPEndPoint> OnConnected;
        public event Action<uint> OnDisconnected;
        public event Action<RequestContext> OnReceived;

        public ConnectionManager(ISequencer clientIdSequencer)
        {
            _clientIdSequencer = clientIdSequencer;
        }

        public void StartServerAsync(string ipAddress, ushort port)
        {
            if (_listener != null)
            {
                throw new ApplicationException("server already started");
            }

            Logger.Log($"starting server {ipAddress}:{port}");
            _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _listener.Start();

            Task.Factory.StartNew(WaitForClientConnection, TaskCreationOptions.LongRunning);
        }

        public void StopServer()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener.Server.Close();
                _listener = null;

                CloseClientConnections();
            }
        }

        private void CloseClientConnections()
        {
            var clients = new List<ClientConnectionManager>();
            lock (_clients)
            {
                clients.AddRange(_clients.Values);
                _clients.Clear();
            }
            foreach (var client in clients)
            {
                client.Stop();
            }
        }

        private void OnClientDisconnected(ClientConnectionManager client)
        {
            lock (_clients)
            {
                _clients.Remove(client.Id);
            }
            OnDisconnected?.Invoke(client.Id);
        }

        private void WaitForClientConnection()
        {
            Logger.Log("server started");
            while (true)
            {
                TcpClient client;
                try
                {
                    client = _listener.AcceptTcpClient();
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode != SocketError.Interrupted)
                    {
                        Logger.LogException(e);
                    }
                    break;
                }
                StartClient(client);
            }
            Logger.Log("server stopped");
        }

        private void StartClient(TcpClient tcpClient)
        {
            var client = new ClientConnectionManager(_clientIdSequencer.Next(), tcpClient, InvokeOnReceivedEvent, OnClientDisconnected);
            client.Logger = Logger;
            lock (_clients)
            {
                _clients.Add(client.Id, client);
            }
            OnConnected?.Invoke(client.Id, (IPEndPoint)tcpClient.Client.LocalEndPoint);
            client.Start();
        }

        private void InvokeOnReceivedEvent(RequestContext requestContext)
        {
            OnReceived?.Invoke(requestContext);
        }
    }
}
