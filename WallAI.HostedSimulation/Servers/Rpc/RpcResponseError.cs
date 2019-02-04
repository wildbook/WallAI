using System;
using System.Runtime.Serialization;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    [DataContract]
    public class RpcResponseError : RpcResponse
    {
        [DataMember(Name = "message")]
        public readonly string Message;

        public RpcResponseError(Guid id, string message) : base(id, RpcResponseType.Error)
        {
            Message = message;
        }
    }
}