using System.IO;

namespace XFlag.Alter3Simulator.Network
{
    public class RequestContext
    {
        public uint ClientId { get; }

        public string ReceivedString { get; }

        public bool IsClose { get; set; }

        public string EndPointString { get; }

        public TextWriter ResponseWriter { get; }

        public RequestContext(uint clientId, string endPointString, string receivedString, TextWriter responseWriter)
        {
            ClientId = clientId;
            EndPointString = endPointString;
            ReceivedString = receivedString;
            ResponseWriter = responseWriter;
        }
    }
}
