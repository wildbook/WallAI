using System;
using System.Runtime.Serialization;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    [DataContract]
    public class RpcResponseEvent<T> : RpcResponse
    {
        [DataMember(Name = "data")]
        public readonly T Data;

        public RpcResponseEvent(Guid id, T data) : base(id, RpcResponseType.Event)
        {
            Data = data;
        }
    }
}