using System.Runtime.Serialization;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    [DataContract]
    public class RpcEvent<T>
    {
        [DataMember(Name = "event")]
        public readonly string Event;

        [DataMember(Name = "data")]
        public readonly T Data;

        public RpcEvent(string eventName, T data)
        {
            Event = eventName;
            Data = data;
        }
    }
}