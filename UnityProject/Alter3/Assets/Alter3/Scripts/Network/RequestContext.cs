using System.Collections.Generic;
using System.Net;

namespace XFlag.Alter3Simulator.Network
{
    public class RequestContext
    {
        public uint ClientId { get; }

        public string ReceivedString { get; }

        public bool IsClose { get; set; }

        public IPEndPoint EndPoint { get; }

        public string[] ResponseLines { get; set; }

        public RequestContext(uint clientId, IPEndPoint endPoint, string receivedString)
        {
            ClientId = clientId;
            EndPoint = endPoint;
            ReceivedString = receivedString;
        }
    }
}
