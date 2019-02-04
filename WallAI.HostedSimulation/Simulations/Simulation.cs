using System;
using System.Linq;
using System.Runtime.Serialization;
using WallAI.Core.Ai;
using WallAI.Core.Math.Geometry;
using WallAI.Core.Worlds;
using WallAI.Core.Worlds.Ai;
using WallAI.HostedSimulation.Simulations.Worlds;

namespace WallAI.HostedSimulation.Simulations
{
    [DataContract]
    public class Simulation
    {
        [DataMember] public Point2D Size => new Point2D(_xSize, _ySize);
        [DataMember] public int CurrentTick { get; protected set; }
        [DataMember] public IWorld2D World { get; }

        public SimulationEvents Events { get; }

        private readonly int _xSize;
        private readonly int _ySize;

        public Simulation(IWorld2DMethods methods, int seed, int xSize, int ySize)
        {
            Events = new SimulationEvents();
            CurrentTick = 0;
            _xSize = xSize;
            _ySize = ySize;
            World = WrappingWorld2D.Create(methods, new Point2D(xSize, ySize), seed);
        }

        public void Tick()
        {
            var aiWorld = new AiWorld2D(World);

            foreach (var entity in World.Entities.Where(x => x.Stats.Alive).ToArray())
                entity.Ai.Tick(new AiCore(aiWorld, entity, CurrentTick));

            CurrentTick++;

            Events.DoTick(CurrentTick);
        }
    }

    public class SimulationEvents
    {
        public event EventHandler<int> Tick;
        public void DoTick(int tick) => Tick?.Invoke(this, tick);
    }
}