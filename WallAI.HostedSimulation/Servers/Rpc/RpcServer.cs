using System;
using System.Collections.Generic;
using System.Linq;
using Fleck;
using Newtonsoft.Json;
using NLog;

namespace WallAI.HostedSimulation.Servers.Rpc
{
    // TODO: The event system is very badly structured, rewrite it.

    public class RpcServer
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        private Dictionary<string, Func<RpcRequest, RpcResponse>> Commands { get; }

        private Dictionary<string, HashSet<Guid>> Events { get; }
        private Dictionary<Guid, Guid> EventIdToClientId { get; }
        private Dictionary<Guid, IWebSocketConnection> Sockets { get; }

        private WebSocketServer Server { get; set; }

        static RpcServer()
        {
            FleckLog.LogAction = (level, message, ex) =>
            {
                switch (level)
                {
                    case Fleck.LogLevel.Debug: Logger.Debug(ex, message); break;
                    case Fleck.LogLevel.Error: Logger.Error(ex, message); break;
                    case Fleck.LogLevel.Warn: Logger.Warn(ex, message); break;
                    case Fleck.LogLevel.Info: Logger.Info(ex, message); break;
                    default: throw new NotSupportedException($"Invalid log level: {level}");
                }
            };
        }

        public RpcServer()
        {
            Sockets = new Dictionary<Guid, IWebSocketConnection>();
            Commands = new Dictionary<string, Func<RpcRequest, RpcResponse>>();
            Events = new Dictionary<string, HashSet<Guid>>();
            EventIdToClientId = new Dictionary<Guid, Guid>();
        }

        public void Start()
        {
            if (Server != null)
                throw new Exception("Server is already running.");

            Server = new WebSocketServer("ws://0.0.0.0:8181")
            {
                SupportedSubProtocols = new[] { "wb-rpc" },
                ListenerSocket = { NoDelay = true },
                RestartAfterListenError = true
            };

            Server.Start(socket =>
            {
                socket.OnOpen += () => Sockets.Add(socket.ConnectionInfo.Id, socket);
                socket.OnClose += () =>
                {
                    var id = socket.ConnectionInfo.Id;

                    Sockets.Remove(id);
                    var eventIds = EventIdToClientId.Where(x => x.Value == id).Select(x => x.Key).ToArray();

                    foreach (var eventId in eventIds)
                    {
                        EventIdToClientId.Remove(eventId);

                        foreach (var ev in Events)
                            ev.Value.RemoveWhere(x => x == eventId);
                    }


                    foreach (var hs in Events.Values)
                        hs.Remove(id);
                };

                socket.OnMessage += message => HandleMessage(socket, message);
            });
        }

        private void HandleMessage(IWebSocketConnection socket, string message)
        {
            var rawRequest = FromString<RpcRequest>(message);
            var socketId = socket.ConnectionInfo.Id;

            switch (rawRequest.Type)
            {
                case RpcRequestType.Subscribe:
                    {
                        var request = FromString<RpcRequestEvent>(message);
                        if (Events.ContainsKey(request.Event))
                        {
                            Logger.Info($"{socketId} subscribed to {request.Event}.");

                            Events[request.Event].Add(request.Id);
                            EventIdToClientId.Add(request.Id, socketId);

                            var response = new RpcResponse(request.Id, RpcResponseType.Subscribed);
                            var responseString = ToString(response);
                            socket.Send(responseString);
                        }
                        else
                        {
                            var response = new RpcResponseError(request.Id, "Event does not exist.");
                            var responseString = ToString(response);
                            socket.Send(responseString);
                        }
                        break;
                    }
                case RpcRequestType.Call:
                    throw new NotImplementedException();
                case RpcRequestType.Unsubscribe:
                    {
                        var request = FromString<RpcRequestEvent>(message);
                        if (Events.ContainsKey(request.Event))
                        {
                            Logger.Info($"Client {socketId} unsubscribed from {request.Event}.");

                            Events[request.Event].Remove(socketId);
                            var response = new RpcResponse(request.Id, RpcResponseType.Unsubscribed);
                            socket.Send(ToString(response));
                        }
                        else
                        {
                            var response = new RpcResponseError(request.Id, "You are not subscribed to this event.");
                            socket.Send(ToString(response));
                        }
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string ToString<T>(T obj) => JsonConvert.SerializeObject(obj);
        private T FromString<T>(string str) => JsonConvert.DeserializeObject<T>(str);

        public void RegisterCommand(string command, Func<RpcRequest, RpcResponse> handler)
        {
            if (Events.ContainsKey(command))
                throw new Exception($"A command is already registered with the name {command}.");

            Commands[command] = handler;
        }

        public void RegisterEvent(string eventName)
        {
            if (Events.ContainsKey(eventName))
                throw new Exception($"An event is already registered with the name {eventName}.");

            Events[eventName] = new HashSet<Guid>();
        }

        public void CallEvent<T>(string eventName, T eventArgs)
        {
            if (!Events.ContainsKey(eventName))
                throw new Exception($"No event exists with the name {eventName}.");

            Logger.Info($"Called event {eventName}.");

            foreach (var eventId in Events[eventName])
            {
                var eventData = new RpcResponseEvent<T>(eventId, eventArgs);
                var eventJson = ToString(eventData);
                Sockets[EventIdToClientId[eventId]].Send(eventJson);
            }
        }

        public void Stop()
        {
            if (Server == null)
                throw new Exception("Server is not running.");

            Server.Dispose();
            Server = null;
        }
    }
}
