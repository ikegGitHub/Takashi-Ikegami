using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    [DisallowMultipleComponent]
    public class ClientConnectionListView : MonoBehaviour
    {
        [SerializeField]
        private ClientConnectionView _clientConnectionViewPrefab = null;

        private readonly Dictionary<uint, ClientConnectionView> _views = new Dictionary<uint, ClientConnectionView>();

        public void Add(uint clientId, EndPoint endPoint)
        {
            if (!_views.ContainsKey(clientId))
            {
                var view = Instantiate(_clientConnectionViewPrefab, transform, false);
                view.Initialize(clientId, endPoint);
                _views.Add(clientId, view);
            }
        }

        public void Remove(uint clientId)
        {
            if (_views.TryGetValue(clientId, out var view))
            {
                _views.Remove(clientId);
                Destroy(view.gameObject);
            }
        }
    }
}
