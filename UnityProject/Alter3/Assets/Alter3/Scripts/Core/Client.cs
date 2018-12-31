using System.Net;

namespace XFlag.Alter3Simulator
{
    public class Client
    {
        public uint Id { get; }

        public IPAddress Address { get; }

        public ClientType Type { get; }

        public string Name { get; set; }

        public Client(uint id, IPAddress address, ClientType type)
        {
            Id = id;
            Address = address;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Name} {Id}@{Address} {Address} {Type}";
        }
    }
}
