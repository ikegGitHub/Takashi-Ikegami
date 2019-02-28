namespace XFlag.Alter3Simulator.Network
{
    public class RequestContext
    {
        public uint ClientId { get; }

        public string ReceivedString { get; }

        public bool IsClose { get; set; }

        public string EndPointString { get; }

        public string[] ResponseLines { get; set; }

        public RequestContext(uint clientId, string endPointString, string receivedString)
        {
            ClientId = clientId;
            EndPointString = endPointString;
            ReceivedString = receivedString;
        }
    }
}
