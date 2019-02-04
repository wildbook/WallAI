namespace WallAI.HostedSimulation.Servers.Rpc
{
    public enum RpcErrorCode
    {
        ParseError = -32700,
        InvalidRequest = -32600,
        MethodNotFound = -32601,
        InvalidParams = -32602,
        InternalError = -32603,
        ServerError = -32000
    }
}