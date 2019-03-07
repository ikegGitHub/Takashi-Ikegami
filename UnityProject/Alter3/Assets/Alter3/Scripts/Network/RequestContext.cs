using System.IO;
using System.Text;

namespace XFlag.Alter3Simulator.Network
{
    public class RequestContext
    {
        public uint ClientId { get; }

        public string ReceivedString { get; }

        public bool IsClose { get; set; }

        public string EndPointString { get; }

        public TextWriter ResponseWriter { get; }

        public RequestContext(uint clientId, string endPointString, string receivedString, Stream responseStream)
        {
            ClientId = clientId;
            EndPointString = endPointString;
            ReceivedString = receivedString;
            ResponseWriter = new StreamWriter(responseStream, Encoding.ASCII);
        }
    }
}
