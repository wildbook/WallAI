using System;
using System.Runtime.Serialization;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    [DataContract]
    public class RpcRequest
    {
        [DataMember(Name = "type")]
        public readonly RpcRequestType Type;

        [DataMember(Name = "id")]
        public readonly Guid Id;

        public RpcRequest(Guid id) => Id = id;
    }
}