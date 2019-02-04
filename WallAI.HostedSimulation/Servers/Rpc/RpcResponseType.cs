namespace WallAI.HostedSimulation.Servers.Rpc
{
    public enum RpcResponseType
    {
        Subscribed = -1,
        Unsubscribed = -2,
        Result = -3,
        Error = -4,
        Notification = -5,
        Event = -6,
    }
}