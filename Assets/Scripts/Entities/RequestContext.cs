using System.IO;
using System.Net;

namespace XFlag.Alter3Simulator
{
    public class RequestContext
    {
        public uint ClientId { get; }

        public string ReceivedString { get; }

        public bool IsClose { get; set; }

        public StreamWriter ResponseWriter { get; }

        public IPEndPoint EndPoint { get; }

        public RequestContext(uint clientId, IPEndPoint endPoint, string receivedString, StreamWriter responseWriter)
        {
            ClientId = clientId;
            EndPoint = endPoint;
            ReceivedString = receivedString;
            ResponseWriter = responseWriter;
        }
    }
}
