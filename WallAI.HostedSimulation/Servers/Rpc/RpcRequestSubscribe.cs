using System;
using System.Runtime.Serialization;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    [DataContract]
    public class RpcRequestEvent : RpcRequest
    {
        [DataMember(Name = "event")]
        public string Event;

        public RpcRequestEvent(Guid id) : base(id) { }
    }
}