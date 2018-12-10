﻿using System.Net;

namespace XFlag.Alter3Simulator
{
    public class ClientConnection
    {
        public uint Id { get; }

        public IPAddress Address { get; }

        public string Type { get; }

        public string Name { get; set; }

        public ClientConnection(uint id, IPAddress address, string type)
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
