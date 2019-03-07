using System.Collections.Generic;
using System.Net;

namespace XFlag.Alter3Simulator
{
    public class CoreSystem
    {
        private Dictionary<uint, Client> _clients = new Dictionary<uint, Client>();

        public IReadOnlyCollection<Client> Clients => _clients.Values;

        public bool IsRobotConnected { get; set; }

        public bool IsRecording { get; set; }

        /// <summary>
        /// 接続されているロボット。接続されていない場合は<code>null</code>。
        /// </summary>
        public IRobot Robot { get; set; }

        public void RegisterClient(uint clientId, IPAddress address, ClientType type)
        {
            var client = new Client(clientId, address, type);
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

        public Client GetClient(uint clientId)
        {
            return _clients[clientId];
        }
    }
}
