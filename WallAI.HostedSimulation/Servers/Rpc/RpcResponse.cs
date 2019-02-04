using System;
using System.Runtime.Serialization;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    [DataContract]
    public class RpcResponse
    {
        [DataMember(Name = "type")]
        public readonly RpcResponseType Type;

        [DataMember(Name = "id")]
        public readonly Guid Id;

        public RpcResponse(Guid id, RpcResponseType type)
        {
            Id = id;
            Type = type;
        }
    }
}