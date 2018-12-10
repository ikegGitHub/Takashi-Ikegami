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

        private uint _clientId = 1;
        private TcpListener _listener;
        private Dictionary<uint, Connection> _connections = new Dictionary<uint, Connection>();

        private uint NextClientId => _clientId++;

        public event Action<uint> OnConnected;
        public event Action<uint> OnDisconnected;
        public event Action<RequestContext> OnReceived;

        public async Task StartServerAsync(string ipAddress, ushort port)
        {
            if (_listener != null)
            {
                throw new ApplicationException("server already started");
            }

            _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _listener.Start();
            Debug.Log("server started");

            await WaitForClientConnection();

            Debug.Log("server stopped");
        }

        public void StopServer()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener = null;
            }
        }

        private async Task WaitForClientConnection()
        {
            while (true)
            {
                TcpClient client;
                try
                {
                    client = await _listener.AcceptTcpClientAsync();
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                var connection = new Connection { id = NextClientId, tcpClient = client };
                OnConnected?.Invoke(connection.id);
                var task = Task.Run(() => WaitForCommand(connection.id, connection.tcpClient));
            }
        }

        private void WaitForCommand(uint clientId, TcpClient client)
        {
            Debug.Log($"[{clientId}] connected");

            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream, Encoding))
            using (var writer = new StreamWriter(stream, Encoding))
            {
                while (client.Connected)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        Debug.Log($"[{clientId}] disconnected");
                        break;
                    }
                    Debug.Log($"[{clientId}] received: {line}");
                    var context = new RequestContext(clientId, line, writer);
                    OnReceived?.Invoke(context);
                    if (context.IsClose)
                    {
                        break;
                    }
                }
            }
            client.Close();
            client.Dispose();
            Debug.Log($"[{clientId}] client end");

            OnDisconnected?.Invoke(clientId);
        }
    }
}
