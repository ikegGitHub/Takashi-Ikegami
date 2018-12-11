using System.Collections.Generic;
using System.Net;

namespace XFlag.Alter3Simulator
{
    public class RequestContext
    {
        public uint ClientId { get; }

        public string ReceivedString { get; }

        public bool IsClose { get; set; }

        public IPEndPoint EndPoint { get; }

        private List<string> _responseLines = new List<string>();

        public IReadOnlyList<string> ResponseLines => _responseLines;

        public RequestContext(uint clientId, IPEndPoint endPoint, string receivedString)
        {
            ClientId = clientId;
            EndPoint = endPoint;
            ReceivedString = receivedString;
        }

        public void AppendResponseLine(string line)
        {
            _responseLines.Add(line);
        }
    }
}
