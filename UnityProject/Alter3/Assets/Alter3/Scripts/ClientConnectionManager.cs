using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class ClientConnectionManager
    {
        private static readonly Encoding Encoding = new UTF8Encoding(false, false);

        private TcpClient _tcpClient;

        private Action<RequestContext> _onRequest;
        private Action _onDisconnected;

        public uint Id { get; }

        public IPEndPoint RemoteEndPoint => (IPEndPoint)_tcpClient.Client.RemoteEndPoint;

        public ILogger Logger { get; set; }

        public ClientConnectionManager(uint id, TcpClient tcpClient, Action<RequestContext> onRequest, Action onDisconnected)
        {
            Id = id;
            _tcpClient = tcpClient;
            _onRequest = onRequest;
            _onDisconnected = onDisconnected;
        }

        public Task Start()
        {
            return StartClientAsync();
        }

        public void Stop()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            if (_tcpClient.Connected)
            {
                _tcpClient.Close();
                _onDisconnected();
            }
        }

        private async Task StartClientAsync()
        {
            Logger?.Log($"[{Id}] client connection started ({RemoteEndPoint}).");
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
                Disconnect();
                Logger?.Log($"[{Id}] client connection finished.");
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

                    Logger?.Log($"[{Id}](req) {line}");

                    var requestContext = new RequestContext(Id, RemoteEndPoint, line);
                    _onRequest(requestContext);

                    foreach (var responseLine in requestContext.ResponseLines)
                    {
                        Logger?.Log($"[{Id}](res) {responseLine}");
                        await writer.WriteLineAsync(responseLine);
                    }
                    await writer.FlushAsync();

                    if (requestContext.IsClose)
                    {
                        break;
                    }
                }
            }
        }
    }
}
