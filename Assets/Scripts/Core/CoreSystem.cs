using System.Collections.Generic;
using System.Net;

namespace XFlag.Alter3Simulator
{
    public class CoreSystem
    {
        private Dictionary<uint, ClientConnection> _clients = new Dictionary<uint, ClientConnection>();

        public IReadOnlyCollection<ClientConnection> Clients => _clients.Values;

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
    }
}
