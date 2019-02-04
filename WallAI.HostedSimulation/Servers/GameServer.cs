using System.Runtime.Serialization;
using WallAI.Core.Worlds;
using WallAI.HostedSimulation.Servers.Rpc;
using WallAI.HostedSimulation.Simulations;

namespace WallAI.HostedSimulation.Servers
{
    [DataContract]
    public class GameServer
    {
        public Simulation Simulation { get; }
        private RpcServer RpcServer { get; }

        public GameServer(Simulation simulation)
        {
            Simulation = simulation;
            RpcServer = new RpcServer();

            RpcServer.RegisterEvent("tick");

            Simulation.Events.Tick += (sender, tick) => RpcServer.CallEvent("tick", Simulation.World as IReadOnlyWorld2D);
        }

        public void Start() => RpcServer.Start();
        public void Stop() => RpcServer.Stop();
    }
}
