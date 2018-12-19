using System.Collections.Generic;
using System.Net;

namespace XFlag.Alter3Simulator
{
    public class CoreSystem
    {
        private Dictionary<uint, ClientConnection> _clients = new Dictionary<uint, ClientConnection>();

        public IReadOnlyCollection<ClientConnection> Clients => _clients.Values;

        public bool IsRobotConnected { get; set; }

        public bool IsRecording { get; set; }

        private double[] _axes;

        public IReadOnlyList<double> Axes => _axes;

        /// <summary>
        /// 接続されているロボット。接続されていない場合は<code>null</code>。
        /// </summary>
        public IRobot Robot { get; set; }

        public CoreSystem(int axisCount)
        {
            _axes = new double[axisCount];
        }

        public void RegisterClient(uint clientId, IPAddress address, ClientType type)
        {
            var client = new ClientConnection(clientId, address, type);
            _clients.Add(client.Id, client);
        }

        public void UnregisterClient(uint clientId)
        {
            _clients.Remove(clientId);
        }

        public void SetClientName(uint clientId, string name)
        {
            var client = _clients[clientId];
            client.Name = name;
        }

        public ClientConnection GetClient(uint clientId)
        {
            return _clients[clientId];
        }

        public double GetAxis(int axisNumber)
        {
            return _axes[axisNumber - 1];
        }
    }
}
