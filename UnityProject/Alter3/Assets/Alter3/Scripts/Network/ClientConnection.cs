using System;
using System.IO;
using System.Net;
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

        public uint Id { get; }

        public IPEndPoint RemoteEndPoint => (IPEndPoint)_tcpClient.Client.RemoteEndPoint;

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

                    Logger?.Log($"[{Id}](req) {line}");

                    var requestContext = new RequestContext(Id, RemoteEndPoint, line);
                    _onRequest(requestContext);

#if false // レスポンスをこのスレッド上で返すとボトルネックになるためオミット
                    foreach (var responseLine in requestContext.ResponseLines)
                    {
                        Logger?.Log($"[{Id}](res) {responseLine}");
                        await writer.WriteLineAsync(responseLine);
                    }
                    await writer.FlushAsync();
#endif

                    if (requestContext.IsClose)
                    {
                        _tcpClient.Close();
                        break;
                    }
                }
            }
        }
    }
}
