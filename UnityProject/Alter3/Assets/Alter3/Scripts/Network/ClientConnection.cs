using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFlag.Alter3Simulator.Network
{
    public class ClientConnection
    {
        private static readonly Encoding Encoding = new UTF8Encoding(false, false);

        private TcpClient _tcpClient;

        private Action<RequestContext> _onRequest;
        private Action<ClientConnection> _onDisconnected;

        private BlockingCollection<string> _logQueue = new BlockingCollection<string>();
        private Task _logTask;

        public uint Id { get; }

        public string RemoteEndPointString { get; private set; }

        public ILogger Logger { get; set; }

        public ClientConnection(uint id, TcpClient tcpClient, Action<RequestContext> onRequest, Action<ClientConnection> onDisconnected)
        {
            if (id == 0)
            {
                throw new ArgumentException("cannot be 0", nameof(id));
            }

            Id = id;
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _onRequest = onRequest ?? throw new ArgumentNullException(nameof(onRequest));
            _onDisconnected = onDisconnected ?? throw new ArgumentNullException(nameof(onDisconnected));
        }

        public Task Start()
        {
            _logTask = Task.Factory.StartNew(OutputLog, TaskCreationOptions.LongRunning);
            return StartClientAsync();
        }

        public void Stop()
        {
            if (_tcpClient.Connected)
            {
                _tcpClient.Close();
            }
        }

        private async Task StartClientAsync()
        {
            RemoteEndPointString = _tcpClient.Client.RemoteEndPoint.ToString();
            Logger?.Log($"[{Id}] client connection started ({RemoteEndPointString}).");
            try
            {
                await WaitForRequestAsync();
            }
            catch (Exception e)
            {
                Logger?.LogException(e);
            }
            finally
            {
                Logger?.Log($"[{Id}] client connection finished.");
                _onDisconnected(this);
            }
        }

        private async Task WaitForRequestAsync()
        {
            using (var stream = _tcpClient.GetStream())
            using (var reader = new StreamReader(stream, Encoding))
            using (var writer = new StreamWriter(stream, Encoding))
            {
                while (_tcpClient.Connected)
                {
                    var line = await reader.ReadLineAsync();
                    if (line == null)
                    {
                        Logger?.Log($"[{Id}] disconnected by peer.");
                        break;
                    }

                    EnqueueLog(line);

                    var requestContext = new RequestContext(Id, RemoteEndPointString, line);
                    _onRequest(requestContext);

                    foreach (var responseLine in requestContext.ResponseLines)
                    {
                        Logger?.Log($"[{Id}](res) {responseLine}");
                        writer.WriteLine(responseLine);
                    }
                    writer.Flush();

                    if (requestContext.IsClose)
                    {
                        _tcpClient.Close();
                        break;
                    }
                }
            }
        }

        private void EnqueueLog(string line)
        {
            _logQueue.Add(line);
        }

        private void OutputLog()
        {
            while (_tcpClient.Connected)
            {
                var line = _logQueue.Take();
                Logger?.Log($"[{Id}](req) {line}");
            }
        }
    }
}
