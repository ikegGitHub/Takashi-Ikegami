using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class ConnectionManager
    {
        private static readonly Encoding Encoding = new UTF8Encoding(false, false);

        private class Connection
        {
            public uint id;
            public TcpClient tcpClient;
        }

        public ILogger Logger { get; set; }

        private uint _clientId = 1;
        private TcpListener _listener;
        private Dictionary<uint, Connection> _connections = new Dictionary<uint, Connection>();

        private uint NextClientId => _clientId++;

        public event Action<uint, IPEndPoint> OnConnected;
        public event Action<uint> OnDisconnected;
        public event Action<RequestContext> OnReceived;

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
            lock (_connections)
            {
                foreach (var connection in _connections.Values)
                {
                    connection.tcpClient.Close();
                }
                _connections.Clear();
            }
        }

        private void OnClientDisconnected(uint clientId)
        {
            lock (_connections)
            {
                _connections.Remove(clientId);
            }
            OnDisconnected?.Invoke(clientId);
        }

        private void WaitForClientConnection()
        {
            Logger.Log("server started");
            while (true)
            {
                TcpClient client;
                try
                {
                    Logger.Log("start accept");
                    client = _listener.AcceptTcpClient();
                    Logger.Log("connected");
                }
                catch (SocketException)
                {
                    break;
                }
                StartClient(client);
            }
            Logger.Log("server stopped");
        }

        private void StartClient(TcpClient tcpClient)
        {
            var connection = new Connection { id = NextClientId, tcpClient = tcpClient };
            lock (_connections)
            {
                _connections.Add(connection.id, connection);
            }
            OnConnected?.Invoke(connection.id, (IPEndPoint)tcpClient.Client.LocalEndPoint);
            Task.Factory.StartNew(() => WaitForCommand(connection.id, connection.tcpClient), TaskCreationOptions.LongRunning);
        }

        private void WaitForCommand(uint clientId, TcpClient client)
        {
            Logger.Log($"[{clientId}] connected endpoint={client.Client.LocalEndPoint}");

            try
            {
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding))
                using (var writer = new StreamWriter(stream, Encoding))
                {
                    while (client.Connected)
                    {
                        var line = reader.ReadLine();
                        if (line == null)
                        {
                            Logger.Log($"[{clientId}] disconnected");
                            break;
                        }
                        Logger.Log($"[{clientId}](req) {line}");
                        var context = new RequestContext(clientId, (IPEndPoint)client.Client.LocalEndPoint, line);
                        OnReceived?.Invoke(context);
                        foreach (var res in context.ResponseLines)
                        {
                            Logger.Log($"[{clientId}](res) {res}");
                            writer.WriteLine(res);
                        }
                        writer.Flush();
                        if (context.IsClose)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                client.Close();
                Logger.Log($"[{clientId}] client end");

                OnClientDisconnected(clientId);
            }
        }
    }
}
