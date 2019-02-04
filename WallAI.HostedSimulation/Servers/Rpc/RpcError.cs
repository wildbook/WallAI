using System;
using System.Runtime.Serialization;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    [DataContract]
    public class RpcError
    {
        [DataMember(Name = "code")]
        public RpcErrorCode Code;

        [DataMember(Name = "message")]
        public string Message;

        [DataMember(Name = "data")]
        public string Data;

        [DataMember(Name = "id")]
        public readonly Guid Id;
    }
}