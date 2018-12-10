using System.IO;

namespace XFlag.Alter3Simulator
{
    public class RequestContext
    {
        public uint ClientId { get; }

        public string ReceivedString { get; }

        public bool IsClose { get; set; }

        public StreamWriter ResponseWriter { get; }

        public RequestContext(uint clientId, string receivedString, StreamWriter responseWriter)
        {
            ClientId = clientId;
            ReceivedString = receivedString;
            ResponseWriter = responseWriter;
        }
    }
}
